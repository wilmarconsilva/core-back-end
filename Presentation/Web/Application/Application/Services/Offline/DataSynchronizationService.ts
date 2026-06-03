import { IListingFilter } from '../../Interfaces/DataTransferObjects/IListingFilter';
import { IOfflineReadRepository } from '../../Interfaces/Persistence/IOfflineReadRepository';
import { IPendingDocumentRepository } from '../../Interfaces/Persistence/IPendingDocumentRepository';
import {
  IDataSynchronizationService,
  SynchronizationListener,
  SynchronizationStatus,
} from '../../Interfaces/Services/IDataSynchronizationService';
import { IProductListing } from '../../Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../../Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { ICustomerListing } from '../../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { IOrderSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IBudgetSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';

const FETCH_PAGE_SIZE = 500;

/** Narrow segregated contracts (ISP): the sync only needs these capabilities. */
interface IDocumentPushService<TSaveDto> {
  create(dto: TSaveDto): Promise<unknown>;
}

interface IListingFetchService<TListing, TFilter extends IListingFilter> {
  getAll(filter: TFilter): Promise<TListing[]>;
}

interface DataSynchronizationDependencies {
  productApi: IListingFetchService<IProductListing, IProductListingFilter>;
  customerApi: IListingFetchService<ICustomerListing, ICustomerListingFilter>;
  orderApi: IDocumentPushService<IOrderSaveDto>;
  budgetApi: IDocumentPushService<IBudgetSaveDto>;
  productRepository: IOfflineReadRepository<IProductListing, IProductListingFilter>;
  customerRepository: IOfflineReadRepository<ICustomerListing, ICustomerListingFilter>;
  pendingOrderRepository: IPendingDocumentRepository<IOrderSaveDto>;
  pendingBudgetRepository: IPendingDocumentRepository<IBudgetSaveDto>;
}

/**
 * Orchestrates startup synchronization following a fixed sequence:
 *   1. push pending Orders   2. push pending Budgets
 *   3. refresh Products      4. refresh Customers
 * Each step is isolated so a failure never interrupts the application; the
 * overall status is exposed to observers (the visual indicator).
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

    await runStep(() => this.pushPendingOrders());
    await runStep(() => this.pushPendingBudgets());
    await runStep(() => this.refreshProducts());
    await runStep(() => this.refreshCustomers());

    this.setStatus(hadError ? 'error' : 'success');
  }

  private async pushPendingOrders(): Promise<void> {
    const pending = await this.dependencies.pendingOrderRepository.getAll();
    for (const record of pending) {
      await this.dependencies.orderApi.create(record.payload);
      await this.dependencies.pendingOrderRepository.remove(record.localId);
    }
  }

  private async pushPendingBudgets(): Promise<void> {
    const pending = await this.dependencies.pendingBudgetRepository.getAll();
    for (const record of pending) {
      await this.dependencies.budgetApi.create(record.payload);
      await this.dependencies.pendingBudgetRepository.remove(record.localId);
    }
  }

  private async refreshProducts(): Promise<void> {
    const products = await this.fetchAll(this.dependencies.productApi);
    await this.dependencies.productRepository.replaceAll(products);
  }

  private async refreshCustomers(): Promise<void> {
    const customers = await this.fetchAll(this.dependencies.customerApi);
    await this.dependencies.customerRepository.replaceAll(customers);
  }

  private async fetchAll<TListing, TFilter extends IListingFilter>(
    service: IListingFetchService<TListing, TFilter>,
  ): Promise<TListing[]> {
    const all: TListing[] = [];
    let start = 0;

    for (;;) {
      const page = await service.getAll({
        registersStart: start,
        registersEnd: start + FETCH_PAGE_SIZE,
      } as TFilter);

      all.push(...page);

      if (page.length < FETCH_PAGE_SIZE) {
        break;
      }
      start += FETCH_PAGE_SIZE;
    }

    return all;
  }

  private setStatus(status: SynchronizationStatus): void {
    this.status = status;
    this.listeners.forEach((listener) => listener(status));
  }
}
