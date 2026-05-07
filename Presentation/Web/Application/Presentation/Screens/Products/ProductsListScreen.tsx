import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { FormField } from '../../Components/FormField';
import { UseDeleteConfirmDialog } from '../../../Application/Hooks/UseDeleteConfirmDialog';
import { UseProducts } from '../../../Application/Hooks/UseProducts';
import { IProductListing } from '../../../Application/Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { ICrudListScreenProps } from '../../Interfaces/Screens/ICrudListScreenProps';
import { layoutStyles, listScreenStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

export function ProductsListScreen({ onGoBack, onNavigateToForm }: ICrudListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove } = UseProducts();
  const [page, setPage] = useState(1);
  const [description, setDescription] = useState('');
  const buildFilter = useCallback((p: number): IProductListingFilter => ({
    registersStart: (p - 1) * PAGE_SIZE,
    registersEnd: p * PAGE_SIZE,
    description: description.trim() || undefined,
  }), [description]);

  useEffect(() => { loadAll(buildFilter(1)); }, []);

  const handleSearch = () => { setPage(1); loadAll(buildFilter(1)); };
  const handlePageChange = (p: number) => { setPage(p); loadAll(buildFilter(p)); };

  const reloadCurrentPage = useCallback(() => {
    loadAll(buildFilter(page));
  }, [loadAll, buildFilter, page]);

  const {
    deleteTarget,
    deleteDialogVisible,
    openDeleteConfirm,
    closeDeleteConfirm,
    confirmDelete,
  } = UseDeleteConfirmDialog<IProductListing>({ remove, reloadAfterDelete: reloadCurrentPage });

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb items={[{ label: 'Início', onPress: onGoBack }, { label: 'Produtos' }]} />
        <Text style={typographyStyles.title}>Produtos</Text>

        <View style={layoutStyles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()}>
            <View style={listScreenStyles.filterFieldWide}>
              <FormField
                label="Descrição"
                placeholder="Filtrar..."
                value={description}
                onChangeText={setDescription}
                onSubmitEditing={handleSearch}
              />
            </View>
          </FilterActionBar>

          <DataGrid<IProductListing>
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
            onDelete={openDeleteConfirm}
            keyExtractor={(item) => String(item.id)}
          />
        </View>
      </ScrollView>

      <ConfirmDialog
        visible={deleteDialogVisible}
        title="Excluir Produto"
        message={`Deseja excluir o produto "${deleteTarget?.description}"?`}
        onConfirm={confirmDelete}
        onCancel={closeDeleteConfirm}
        isLoading={isDeleting}
        confirmLabel="Excluir"
      />
    </View>
  );
}
