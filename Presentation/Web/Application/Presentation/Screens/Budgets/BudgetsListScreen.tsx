import { Ionicons } from '@expo/vector-icons';
import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { DateField } from '../../Components/DateField';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { UseBudgets } from '../../../Application/Hooks/UseBudgets';
import { IBudgetListing } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListing';
import { IBudgetListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetListingFilter';
import { ICrudListScreenProps } from '../../Interfaces/Screens/ICrudListScreenProps';
import { colors, layoutStyles, listScreenStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

export function BudgetsListScreen({ onGoBack, onNavigateToForm }: ICrudListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove, isCreatingOrder, createOrderByBudget } = UseBudgets();
  const [page, setPage] = useState(1);
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [deleteTarget, setDeleteTarget] = useState<IBudgetListing | null>(null);

  const buildFilter = useCallback((p: number): IBudgetListingFilter => ({
    registersStart: (p - 1) * PAGE_SIZE,
    registersEnd: p * PAGE_SIZE,
    creationDateTimeStart: startDate || undefined,
    creationDateTimeEnd: endDate || undefined,
  }), [startDate, endDate]);

  useEffect(() => { loadAll(buildFilter(1)); }, []);

  const handleSearch = () => { setPage(1); loadAll(buildFilter(1)); };
  const handlePageChange = (p: number) => { setPage(p); loadAll(buildFilter(p)); };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    const success = await remove(deleteTarget.id);
    if (success) { setDeleteTarget(null); loadAll(buildFilter(page)); }
  };

  const handleCreateOrder = async (budgetId: number) => {
    await createOrderByBudget(budgetId);
    loadAll(buildFilter(page));
  };

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb items={[{ label: 'Início', onPress: onGoBack }, { label: 'Orçamentos' }]} />
        <Text style={typographyStyles.title}>Orçamentos</Text>

        <View style={layoutStyles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()}>
            <View style={listScreenStyles.filterFieldCompact}>
              <DateField
                label="Data início"
                value={startDate}
                onChangeText={setStartDate}
                onSubmitEditing={handleSearch}
              />
            </View>
            <View style={listScreenStyles.filterFieldCompact}>
              <DateField
                label="Data fim"
                value={endDate}
                onChangeText={setEndDate}
                onSubmitEditing={handleSearch}
              />
            </View>
          </FilterActionBar>

          <DataGrid<IBudgetListing>
            columns={[
              { key: 'id', label: 'Código', flex: 0.4 },
              { key: 'custormerName', label: 'Cliente', flex: 2 },
              { key: 'customerCityState', label: 'Cidade/Estado', flex: 1.5 },
              {
                key: 'valueTotal',
                label: 'Total',
                flex: 1,
                render: (item) => (
                  <Text style={listScreenStyles.currency}>
                    {item.valueTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                  </Text>
                ),
              },
            ]}
            data={listings}
            isLoading={isLoadingList}
            page={page}
            pageSize={PAGE_SIZE}
            onPageChange={handlePageChange}
            onEdit={(item) => onNavigateToForm(item.id)}
            onDelete={(item) => setDeleteTarget(item)}
            keyExtractor={(item) => String(item.id)}
            extraActions={(item) => (
              <TouchableOpacity
                onPress={() => handleCreateOrder(item.id)}
                activeOpacity={0.7}
                disabled={isCreatingOrder}
              >
                <Ionicons name="clipboard-outline" size={18} color={colors.primary} />
              </TouchableOpacity>
            )}
          />
        </View>
      </ScrollView>

      <ConfirmDialog
        visible={!!deleteTarget}
        title="Excluir Orçamento"
        message={`Deseja excluir o orçamento ${deleteTarget?.id}?`}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(null)}
        isLoading={isDeleting}
        confirmLabel="Excluir"
      />
    </View>
  );
}

