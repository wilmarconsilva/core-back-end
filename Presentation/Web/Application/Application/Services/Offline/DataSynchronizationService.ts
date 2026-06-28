import { IListingFilter } from '../../Interfaces/DataTransferObjects/IListingFilter';
import { IDocumentSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/IDocumentSaveDto';
import { IDocumentProductSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/IDocumentProductSaveDto';
import { IOfflineRepository } from '../../../Infrastructure/Persistence/Interfaces/IOfflineRepository';
import {
  IDataSynchronizationService,
  SynchronizationListener,
  SynchronizationStatus,
} from '../../Interfaces/Services/IDataSynchronizationService';
import { IProductListing } from '../../Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../../Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { IProductSaveDto } from '../../Interfaces/DataTransferObjects/Products/IProductSaveDto';
import { ICustomerListing } from '../../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { ICustomerSaveDto } from '../../Interfaces/DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { IOrderSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IBudgetSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { ILocalityListing } from '../../Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../../Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';
import { ILocalitySaveDto } from '../../Interfaces/DataTransferObjects/Localities/ILocalitySaveDto';
import { toApiCreatePayload } from '../Utilities/OfflineUtilities';
import { remapDocumentReferences } from './OfflineMappersService';

const FETCH_PAGE_SIZE = 50;

interface ICreatePushService<TSaveDto> {
  create(dto: TSaveDto): Promise<TSaveDto>;
}

interface IListingFetchService<TListing, TFilter extends IListingFilter> {
  getAll(filter: TFilter): Promise<TListing[]>;
}

interface DataSynchronizationDependencies {
  productApi: IListingFetchService<IProductListing, IProductListingFilter> & ICreatePushService<IProductSaveDto>;
  customerApi: IListingFetchService<ICustomerListing, ICustomerListingFilter> & ICreatePushService<ICustomerSaveDto>;
  orderApi: ICreatePushService<IOrderSaveDto>;
  budgetApi: ICreatePushService<IBudgetSaveDto>;
  localityApi: IListingFetchService<ILocalityListing, ILocalityListingFilter>;
  productRepository: IOfflineRepository<IProductSaveDto, IProductListing, IProductListingFilter>;
  customerRepository: IOfflineRepository<ICustomerSaveDto, ICustomerListing, ICustomerListingFilter>;
  orderRepository: IOfflineRepository<IOrderSaveDto, unknown, unknown>;
  budgetRepository: IOfflineRepository<IBudgetSaveDto, unknown, unknown>;
  localityRepository: IOfflineRepository<ILocalitySaveDto, ILocalityListing, ILocalityListingFilter>;
}

/**
 * Orchestrates startup synchronization following a fixed sequence:
 *   1. push local Products    2. push local Customers
 *   3. remap FKs              4. push local Orders
 *   5. push local Budgets     6. refresh Products
 *   7. refresh Customers      8. refresh Localities
 */
export class DataSynchronizationService implements IDataSynchronizationService {
  private status: SynchronizationStatus = 'idle';
  private readonly listeners = new Set<SynchronizationListener>();
  private hasRun = false;

  constructor(private readonly dependencies: DataSynchronizationDependencies) {}

  getStatus(): SynchronizationStatus {
    return this.status;
  }

  subscribe(listener: SynchronizationListener): () => void {
    this.listeners.add(listener);
    listener(this.status);
    return () => {
      this.listeners.delete(listener);
    };
  }

  async synchronize(): Promise<void> {
    if (this.hasRun || this.status === 'syncing') {
      return;
    }
    this.hasRun = true;
    this.setStatus('syncing');

    let hadError = false;
    const runStep = async (step: () => Promise<void>): Promise<void> => {
      try {
        await step();
      } catch {
        hadError = true;
      }
    };

    let productIdMap = new Map<number, number>();
    let customerIdMap = new Map<number, number>();

    await runStep(async () => {
      productIdMap = await this.pushLocalRecords(
        this.dependencies.productRepository,
        this.dependencies.productApi,
      );
    });
    await runStep(async () => {
      customerIdMap = await this.pushLocalRecords(
        this.dependencies.customerRepository,
        this.dependencies.customerApi,
      );
    });
    await runStep(() => this.pushLocalDocuments(
      this.dependencies.orderRepository,
      this.dependencies.orderApi,
      customerIdMap,
      productIdMap,
    ));
    await runStep(() => this.pushLocalDocuments(
      this.dependencies.budgetRepository,
      this.dependencies.budgetApi,
      customerIdMap,
      productIdMap,
    ));
    await runStep(() => this.refreshProducts());
    await runStep(() => this.refreshCustomers());
    await runStep(() => this.refreshLocalities());

    this.setStatus(hadError ? 'error' : 'success');
  }

  private async pushLocalRecords<T extends { id: number }>(
    repository: IOfflineRepository<T, unknown, unknown>,
    api: ICreatePushService<T>,
  ): Promise<Map<number, number>> {
    const idMap = new Map<number, number>();
    const pending = await repository.getLocalRecords();

    for (const record of pending) {
      const created = await api.create(toApiCreatePayload(record));
      idMap.set(record.id, created.id);
      await repository.remove(record.id);
    }

    return idMap;
  }

  private async pushLocalDocuments<T extends IDocumentSaveDto<IDocumentProductSaveDto>>(
    repository: IOfflineRepository<T, unknown, unknown>,
    api: ICreatePushService<T>,
    customerIdMap: Map<number, number>,
    productIdMap: Map<number, number>,
  ): Promise<void> {
    const pending = await repository.getLocalRecords();

    for (const record of pending) {
      const remapped = remapDocumentReferences(record, customerIdMap, productIdMap);
      await api.create(toApiCreatePayload(remapped));
      await repository.remove(record.id);
    }
  }

  private async refreshProducts(): Promise<void> {
    const products = await this.fetchAll(this.dependencies.productApi);
    await this.dependencies.productRepository.replaceSynced(products);
  }

  private async refreshCustomers(): Promise<void> {
    const customers = await this.fetchAll(this.dependencies.customerApi);
    await this.dependencies.customerRepository.replaceSynced(customers);
  }

  private async refreshLocalities(): Promise<void> {
    const localities = await this.fetchAll(this.dependencies.localityApi);
    await this.dependencies.localityRepository.replaceSynced(localities);
  }

  private async fetchAll<TListing, TFilter extends IListingFilter>(
    service: IListingFetchService<TListing, TFilter>,
  ): Promise<TListing[]> {
    const all: TListing[] = [];
    let start = 0;
    let page: TListing[] | null = null;

    while(page === null || page.length === FETCH_PAGE_SIZE) {
      page = await service.getAll({
        registersStart: start,
        registersEnd: start + FETCH_PAGE_SIZE,
      } as TFilter);

      all.push(...page);

      start += FETCH_PAGE_SIZE;
    }

    return all;
  }

  private setStatus(status: SynchronizationStatus): void {
    this.status = status;
    this.listeners.forEach((listener) => listener(status));
  }
}
