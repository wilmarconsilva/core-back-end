import { Ionicons } from '@expo/vector-icons';
import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { DateField } from '../../Components/DateField';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { UseBudgets } from '../../../Application/Hooks/UseBudgets';
import { BudgetListing } from '../../../Application/Interfaces/DataTransferObjects/BudgetListing';
import { BudgetListingFilter } from '../IListingFilter';
import { colors, layoutStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

interface BudgetsListScreenProps {
  onGoBack: () => void;
  onNavigateToForm: (id?: number) => void;
}

const styles = StyleSheet.create({
  filterField: { flex: 1, minWidth: 140 },
  sectionCard: {
    backgroundColor: colors.surface,
    borderRadius: 12,
    padding: 16,
    width: '100%',
    shadowColor: colors.shadow,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.06,
    shadowRadius: 4,
    elevation: 2,
  },
  currency: { fontSize: 13, color: colors.textPrimary },
});

export function BudgetsListScreen({ onGoBack, onNavigateToForm }: BudgetsListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove, isCreatingOrder, createOrderByBudget } = UseBudgets();
  const [page, setPage] = useState(1);
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [deleteTarget, setDeleteTarget] = useState<BudgetListing | null>(null);

  const buildFilter = useCallback((p: number): BudgetListingFilter => ({
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

        <View style={styles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()}>
            <View style={styles.filterField}>
              <DateField label="Data início" value={startDate} onChangeText={setStartDate} />
            </View>
            <View style={styles.filterField}>
              <DateField label="Data fim" value={endDate} onChangeText={setEndDate} />
            </View>
          </FilterActionBar>

          <DataGrid<BudgetListing>
            columns={[
              { key: 'id', label: 'Código', flex: 0.4 },
              { key: 'custormerName', label: 'Cliente', flex: 2 },
              { key: 'customerCityState', label: 'Cidade/Estado', flex: 1.5 },
              {
                key: 'valueTotal',
                label: 'Total',
                flex: 1,
                render: (item) => (
                  <Text style={styles.currency}>
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

