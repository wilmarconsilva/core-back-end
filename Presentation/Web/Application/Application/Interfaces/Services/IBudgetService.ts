import { ICrudService } from './ICrudService';
import { IBudgetSaveDto } from '../DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { IBudgetListing } from '../DataTransferObjects/Commercial/Documents/Budgets/IBudgetListing';
import { IBudgetListingFilter } from '../DataTransferObjects/Commercial/Documents/Budgets/IBudgetListingFilter';
import { IOrderSaveDto } from '../DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';

export interface IBudgetService extends ICrudService<IBudgetSaveDto, IBudgetListing, IBudgetListingFilter> {
  createOrderByBudget(id: number): Promise<IOrderSaveDto>;
}
