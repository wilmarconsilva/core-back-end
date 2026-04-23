import { useCallback, useMemo, useState } from 'react';
import { BudgetService } from '../Services/BudgetService';
import { BudgetSaveDto } from '../Interfaces/DataTransferObjects/BudgetSaveDto';
import { BudgetListing } from '../Interfaces/DataTransferObjects/BudgetListing';
import { BudgetListingFilter } from '../Interfaces/DataTransferObjects/BudgetListingFilter';
import { OrderSaveDto } from '../Interfaces/DataTransferObjects/OrderSaveDto';
import { UseCrud, UseCrudResult } from './UseCrud';
import Toast from 'react-native-toast-message';

const budgetService = new BudgetService();

export type UseBudgetsResult = UseCrudResult<BudgetSaveDto, BudgetListing, BudgetListingFilter> & {
  isCreatingOrder: boolean;
  createdOrder: OrderSaveDto | null;
  createOrderByBudget: (budgetId: number) => Promise<boolean>;
};

export function UseBudgets(): UseBudgetsResult {
  const service = useMemo(() => budgetService, []);
  const crud = UseCrud(service);
  const [isCreatingOrder, setIsCreatingOrder] = useState(false);
  const [createdOrder, setCreatedOrder] = useState<OrderSaveDto | null>(null);

  const createOrderByBudget = useCallback(async (budgetId: number): Promise<boolean> => {
    setIsCreatingOrder(true);
    try {
      const order = await budgetService.createOrderByBudget(budgetId);
      setCreatedOrder(order);
      Toast.show({ type: 'success', text1: 'Pedido criado com sucesso!' });
      return true;
    } catch {
      return false;
    } finally {
      setIsCreatingOrder(false);
    }
  }, []);

  return { ...crud, isCreatingOrder, createdOrder, createOrderByBudget };
}
