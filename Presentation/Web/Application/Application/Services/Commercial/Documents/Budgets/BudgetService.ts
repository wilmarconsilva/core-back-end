import { CrudService } from '../../../CrudService';
import { IBudgetService } from '../../../../Interfaces/Services/IBudgetService';
import { IBudgetSaveDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { IBudgetDetailDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetDetailDto';
import { IBudgetListing } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListing';
import { IBudgetListingFilter } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListingFilter';
import { IOrderDetailDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderDetailDto';

export class BudgetService
  extends CrudService<IBudgetSaveDto, IBudgetDetailDto, IBudgetListing, IBudgetListingFilter>
  implements IBudgetService
{
  constructor() {
    super('/budgets');
  }

  async createOrderByBudget(id: number): Promise<IOrderDetailDto> {
    return this.request<IOrderDetailDto>(`/budgets/${id}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
