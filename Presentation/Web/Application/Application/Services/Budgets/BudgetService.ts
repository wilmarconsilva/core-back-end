import { CrudService } from '../CrudService';
import { IBudgetService } from '../../Interfaces/Services/IBudgetService';
import { BudgetSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { BudgetListing } from '../../Interfaces/DataTransferObjects/BudgetListing';
import { BudgetListingFilter } from '../IListingFilter';
import { IOrderSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';

export class BudgetService
  extends CrudService<BudgetSaveDto, BudgetListing, BudgetListingFilter>
  implements IBudgetService
{
  constructor() {
    super('/budgets');
  }

  async createOrderByBudget(id: number): Promise<OrderSaveDto> {
    return this.request<OrderSaveDto>(`/budgets/${id}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
