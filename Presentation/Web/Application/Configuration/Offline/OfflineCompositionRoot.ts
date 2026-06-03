import { ICrudService } from '../../Application/Interfaces/Services/ICrudService';
import { IOfflineReadRepository } from '../../Application/Interfaces/Persistence/IOfflineReadRepository';
import { IPendingDocumentRepository } from '../../Application/Interfaces/Persistence/IPendingDocumentRepository';
import { OfflineCrudService } from '../../Application/Services/Offline/OfflineCrudService';
import { OfflineDocumentService } from '../../Application/Services/Offline/OfflineDocumentService';
import { DataSynchronizationService } from '../../Application/Services/Offline/DataSynchronizationService';
import { IDataSynchronizationService } from '../../Application/Interfaces/Services/IDataSynchronizationService';
import { ProductService } from '../../Application/Services/Products/ProductService';
import { CustomerService } from '../../Application/Services/Customers/CustomerService';
import { OrderService } from '../../Application/Services/Commercial/Documents/Orders/OrderService';
import { BudgetService } from '../../Application/Services/Commercial/Documents/Budgets/BudgetService';
import { IProductListing } from '../../Application/Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../../Application/Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { IProductSaveDto } from '../../Application/Interfaces/DataTransferObjects/Products/IProductSaveDto';
import { IProductDetailDto } from '../../Application/Interfaces/DataTransferObjects/Products/IProductDetailDto';
import { ICustomerListing } from '../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { ICustomerSaveDto } from '../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { ICustomerDetailDto } from '../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerDetailDto';
import { IOrderSaveDto } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IOrderDetailDto } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderDetailDto';
import { IOrderListing } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListing';
import { IOrderListingFilter } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListingFilter';
import { IBudgetSaveDto } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { IBudgetDetailDto } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetDetailDto';
import { IBudgetListing } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListing';
import { IBudgetListingFilter } from '../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListingFilter';
import { IndexedDbContext } from '../../Infrastructure/Persistence/IndexedDb/IndexedDbContext';
import { IndexedDbReadRepository } from '../../Infrastructure/Persistence/IndexedDb/IndexedDbReadRepository';
import { IndexedDbPendingRepository } from '../../Infrastructure/Persistence/IndexedDb/IndexedDbPendingRepository';
import { OfflineStoreNames } from '../../Infrastructure/Persistence/IndexedDb/IndexedDbSchema';

function containsIgnoreCase(value: string, term?: string): boolean {
  if (!term) return true;
  return value.toLowerCase().includes(term.toLowerCase());
}

function matchesId(id: number, filterId?: number): boolean {
  return filterId === undefined || id === filterId;
}

/**
 * Composition root that wires the IndexedDB persistence into the offline-aware
 * services. The repositories are exposed so the synchronization service can
 * reuse the very same instances. Everything is created once (singletons).
 */
class OfflineCompositionRootImpl {
  private readonly context = IndexedDbContext.getInstance();

  readonly productRepository: IOfflineReadRepository<IProductListing, IProductListingFilter> =
    new IndexedDbReadRepository<IProductListing, IProductListingFilter>(
      this.context,
      OfflineStoreNames.products,
      (item, filter) =>
        matchesId(item.id, filter.id) && containsIgnoreCase(item.description, filter.description),
    );

  readonly customerRepository: IOfflineReadRepository<ICustomerListing, ICustomerListingFilter> =
    new IndexedDbReadRepository<ICustomerListing, ICustomerListingFilter>(
      this.context,
      OfflineStoreNames.customers,
      (item, filter) =>
        matchesId(item.id, filter.id) && containsIgnoreCase(item.name, filter.name),
    );

  readonly pendingOrderRepository: IPendingDocumentRepository<IOrderSaveDto> =
    new IndexedDbPendingRepository<IOrderSaveDto>(this.context, OfflineStoreNames.pendingOrders);

  readonly pendingBudgetRepository: IPendingDocumentRepository<IBudgetSaveDto> =
    new IndexedDbPendingRepository<IBudgetSaveDto>(this.context, OfflineStoreNames.pendingBudgets);

  readonly dataSynchronizationService: IDataSynchronizationService =
    new DataSynchronizationService({
      productApi: new ProductService(),
      customerApi: new CustomerService(),
      orderApi: new OrderService(),
      budgetApi: new BudgetService(),
      productRepository: this.productRepository,
      customerRepository: this.customerRepository,
      pendingOrderRepository: this.pendingOrderRepository,
      pendingBudgetRepository: this.pendingBudgetRepository,
    });

  createProductService(): ICrudService<IProductSaveDto, IProductDetailDto, IProductListing, IProductListingFilter> {
    return new OfflineCrudService(new ProductService(), this.productRepository);
  }

  createCustomerService(): ICrudService<ICustomerSaveDto, ICustomerDetailDto, ICustomerListing, ICustomerListingFilter> {
    return new OfflineCrudService(new CustomerService(), this.customerRepository);
  }

  createOrderService(): ICrudService<IOrderSaveDto, IOrderDetailDto, IOrderListing, IOrderListingFilter> {
    return new OfflineDocumentService(new OrderService(), this.pendingOrderRepository);
  }

  createBudgetService(): ICrudService<IBudgetSaveDto, IBudgetDetailDto, IBudgetListing, IBudgetListingFilter> {
    return new OfflineDocumentService(new BudgetService(), this.pendingBudgetRepository);
  }
}

export const OfflineCompositionRoot = new OfflineCompositionRootImpl();
