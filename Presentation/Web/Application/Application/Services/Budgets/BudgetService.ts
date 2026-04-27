import { CrudService } from '../CrudService';
import { IBudgetService } from '../../Interfaces/Services/IBudgetService';
import { IBudgetSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { IBudgetListing } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListing';
import { IBudgetListingFilter } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListingFilter';
import { IOrderSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';

export class BudgetService
  extends CrudService<IBudgetSaveDto, IBudgetListing, IBudgetListingFilter>
  implements IBudgetService
{
  constructor() {
    super('/budgets');
  }

  async createOrderByBudget(id: number): Promise<IOrderSaveDto> {
    return this.request<IOrderSaveDto>(`/budgets/${id}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
