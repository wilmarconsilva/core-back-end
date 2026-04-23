import { ICrudService } from './ICrudService';
import { BudgetSaveDto } from '../DataTransferObjects/BudgetSaveDto';
import { BudgetListing } from '../DataTransferObjects/BudgetListing';
import { BudgetListingFilter } from '../DataTransferObjects/BudgetListingFilter';
import { OrderSaveDto } from '../DataTransferObjects/OrderSaveDto';

export interface IBudgetService extends ICrudService<BudgetSaveDto, BudgetListing, BudgetListingFilter> {
  createOrderByBudget(id: number): Promise<OrderSaveDto>;
}
