import { useCallback, useMemo, useState } from 'react';
import { BudgetService } from '../Services/Budgets/BudgetService';
import { IBudgetSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { IBudgetListing } from '../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListing';
import { IBudgetListingFilter } from '../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListingFilter';
import { IOrderSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { UseCrud, UseCrudResult } from './UseCrud';
import Toast from 'react-native-toast-message';

const budgetService = new BudgetService();

export type UseBudgetsResult = UseCrudResult<IBudgetSaveDto, IBudgetListing, IBudgetListingFilter> & {
  isCreatingOrder: boolean;
  createdOrder: IOrderSaveDto | null;
  createOrderByBudget: (budgetId: number) => Promise<boolean>;
};

export function UseBudgets(): UseBudgetsResult {
  const service = useMemo(() => budgetService, []);
  const crud = UseCrud(service);
  const [isCreatingOrder, setIsCreatingOrder] = useState(false);
  const [createdOrder, setCreatedOrder] = useState<IOrderSaveDto | null>(null);

  const createOrderByBudget = useCallback(async (budgetId: number): Promise<boolean> => {
    setIsCreatingOrder(true);
    try {
      const order = await budgetService.createOrderByBudget(budgetId);
      setCreatedOrder(order);
      return true;
    } catch {
      return false;
    } finally {
      setIsCreatingOrder(false);
    }
  }, []);

  return { ...crud, isCreatingOrder, createdOrder, createOrderByBudget };
}
