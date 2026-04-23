import { CrudService } from './CrudService';
import { IBudgetService } from '../Interfaces/Services/IBudgetService';
import { BudgetSaveDto } from '../Interfaces/DataTransferObjects/BudgetSaveDto';
import { BudgetListing } from '../Interfaces/DataTransferObjects/BudgetListing';
import { BudgetListingFilter } from '../Interfaces/DataTransferObjects/BudgetListingFilter';
import { OrderSaveDto } from '../Interfaces/DataTransferObjects/OrderSaveDto';
import { TokenStorage } from './TokenStorage';

export class BudgetService
  extends CrudService<BudgetSaveDto, BudgetListing, BudgetListingFilter>
  implements IBudgetService
{
  constructor() {
    super('/budgets');
  }

  async createOrderByBudget(id: number): Promise<OrderSaveDto> {
    const token = TokenStorage.getToken();
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    };
    return this.authorizedRequest<OrderSaveDto>(`/budgets/${id}/orders`, {
      method: 'POST',
      credentials: 'include',
      headers,
    });
  }
}
