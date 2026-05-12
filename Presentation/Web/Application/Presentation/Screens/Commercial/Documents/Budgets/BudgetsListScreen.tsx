import { Ionicons } from '@expo/vector-icons';
import React, { useCallback, useState } from 'react';
import { TouchableOpacity } from 'react-native';
import { UsePagedCrudList } from '../../../../../Application/Hooks/UsePagedCrudList';
import { UseBudgets } from '../../../../../Application/Hooks/UseBudgets';
import { IBudgetListing } from '../../../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListing';
import { IBudgetListingFilter } from '../../../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListingFilter';
import { ICrudListScreenProps } from '../../../../Interfaces/Screens/ICrudListScreenProps';
import { colors } from '../../../../Styles';
import { CommercialDocumentListScreen } from '../CommercialDocumentListScreen';

export function BudgetsListScreen({ onGoBack, onNavigateToForm }: ICrudListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove, isCreatingOrder, createOrderByBudget } = UseBudgets();
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');

  const buildFilter = useCallback((p: number, pageSize: number): IBudgetListingFilter => ({
    registersStart: (p - 1) * pageSize,
    registersEnd: p * pageSize,
    creationDateTimeStart: startDate || undefined,
    creationDateTimeEnd: endDate || undefined,
  }), [startDate, endDate]);

  const listState = UsePagedCrudList<IBudgetListing, IBudgetListingFilter>({
    loadAll,
    remove,
    buildFilter,
  });

  const handleCreateOrder = async (budgetId: number) => {
    await createOrderByBudget(budgetId);
    listState.reloadCurrentPage();
  };

  return (
    <CommercialDocumentListScreen<IBudgetListing>
      title="Orçamentos"
      listings={listings}
      isLoadingList={isLoadingList}
      isDeleting={isDeleting}
      listState={listState}
      startDate={startDate}
      setStartDate={setStartDate}
      endDate={endDate}
      setEndDate={setEndDate}
      deleteTitle="Excluir Orçamento"
      getDeleteMessage={(item) => `Deseja excluir o orçamento ${item?.id}?`}
      showCreationDate
      extraActions={(item) => (
        <TouchableOpacity
          onPress={() => handleCreateOrder(item.id)}
          activeOpacity={0.7}
          disabled={isCreatingOrder}
        >
          <Ionicons name="clipboard-outline" size={18} color={colors.primary} />
        </TouchableOpacity>
      )}
      onGoBack={onGoBack}
      onNavigateToForm={onNavigateToForm}
    />
  );
}

