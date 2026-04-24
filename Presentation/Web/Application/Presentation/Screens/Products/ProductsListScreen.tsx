import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, StyleSheet, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { FormField } from '../../Components/FormField';
import { UseProducts } from '../../../Application/Hooks/UseProducts';
import { ProductListing } from '../../../Application/Interfaces/DataTransferObjects/ProductListing';
import { ProductListingFilter } from '../../../Application/Interfaces/DataTransferObjects/ProductListingFilter';
import { colors, layoutStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

interface ProductsListScreenProps {
  onGoBack: () => void;
  onNavigateToForm: (id?: number) => void;
}

const styles = StyleSheet.create({
  filterField: { flex: 1, minWidth: 180 },
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
});

export function ProductsListScreen({ onGoBack, onNavigateToForm }: ProductsListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove } = UseProducts();
  const [page, setPage] = useState(1);
  const [description, setDescription] = useState('');
  const [deleteTarget, setDeleteTarget] = useState<ProductListing | null>(null);

  const buildFilter = useCallback((p: number): ProductListingFilter => ({
    registersStart: (p - 1) * PAGE_SIZE,
    registersEnd: p * PAGE_SIZE,
    description: description.trim() || undefined,
  }), [description]);

  useEffect(() => { loadAll(buildFilter(1)); }, []);

  const handleSearch = () => { setPage(1); loadAll(buildFilter(1)); };
  const handlePageChange = (p: number) => { setPage(p); loadAll(buildFilter(p)); };

  const handleDelete = async () => {
    if (!deleteTarget) return;
    const success = await remove(deleteTarget.id);
    if (success) { setDeleteTarget(null); loadAll(buildFilter(page)); }
  };

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainerList}>
        <Breadcrumb items={[{ label: 'Início', onPress: onGoBack }, { label: 'Produtos' }]} />
        <Text style={typographyStyles.title}>Produtos</Text>

        <View style={styles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()}>
            <View style={styles.filterField}>
              <FormField
                label="Descrição"
                placeholder="Filtrar..."
                value={description}
                onChangeText={setDescription}
                onSubmitEditing={handleSearch}
              />
            </View>
          </FilterActionBar>

          <DataGrid<ProductListing>
            columns={[
              { key: 'id', label: 'Código', flex: 0.4 },
              { key: 'description', label: 'Descrição', flex: 2 },
            ]}
            data={listings}
            isLoading={isLoadingList}
            page={page}
            pageSize={PAGE_SIZE}
            onPageChange={handlePageChange}
            onEdit={(item) => onNavigateToForm(item.id)}
            onDelete={(item) => setDeleteTarget(item)}
            keyExtractor={(item) => String(item.id)}
          />
        </View>
      </ScrollView>

      <ConfirmDialog
        visible={!!deleteTarget}
        title="Excluir Produto"
        message={`Deseja excluir o produto "${deleteTarget?.description}"?`}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(null)}
        isLoading={isDeleting}
        confirmLabel="Excluir"
      />
    </View>
  );
}
