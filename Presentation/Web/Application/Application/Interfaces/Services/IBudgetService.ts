import { ICrudService } from './ICrudService';
import { IBudgetSaveDto } from '../DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { IBudgetListing } from '../DataTransferObjects/IBudgetListing';
import { IBudgetListingFilter } from '../IListingFilter';
import { IOrderSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';

export interface IBudgetService extends ICrudService<IBudgetSaveDto, IBudgetListing, IBudgetListingFilter> {
  createOrderByBudget(id: number): Promise<IOrderSaveDto>;
}
