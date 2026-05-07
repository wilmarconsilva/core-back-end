import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { FormField } from '../../Components/FormField';
import { UseDeleteConfirmDialog } from '../../../Application/Hooks/UseDeleteConfirmDialog';
import { UseLocalities } from '../../../Application/Hooks/UseLocalities';
import { ILocalityListing } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';
import { ICrudListScreenProps } from '../../Interfaces/Screens/ICrudListScreenProps';
import { layoutStyles, listScreenStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

export function LocalitiesListScreen({ onGoBack, onNavigateToForm }: ICrudListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove } = UseLocalities();
  const [page, setPage] = useState(1);
  const [description, setDescription] = useState('');
  const buildFilter = useCallback((p: number): ILocalityListingFilter => ({
    registersStart: (p - 1) * PAGE_SIZE,
    registersEnd: p * PAGE_SIZE,
    description: description.trim() || undefined,
  }), [description]);

  useEffect(() => {
    loadAll(buildFilter(page));
  }, []);

  const handleSearch = () => {
    setPage(1);
    loadAll(buildFilter(1));
  };

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
    loadAll(buildFilter(newPage));
  };

  const reloadCurrentPage = useCallback(() => {
    loadAll(buildFilter(page));
  }, [loadAll, buildFilter, page]);

  const {
    deleteTarget,
    deleteDialogVisible,
    openDeleteConfirm,
    closeDeleteConfirm,
    confirmDelete,
  } = UseDeleteConfirmDialog<ILocalityListing>({ remove, reloadAfterDelete: reloadCurrentPage });

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb
          items={[{ label: 'Início', onPress: onGoBack }, { label: 'Localidades' }]}
        />
        <Text style={typographyStyles.title}>Localidades</Text>

        <View style={layoutStyles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()} newLabel="+ Nova">
            <View style={listScreenStyles.filterFieldFlush}>
              <FormField
                label="Descrição"
                placeholder="Filtrar por descrição..."
                value={description}
                onChangeText={setDescription}
                onSubmitEditing={handleSearch}
              />
            </View>
          </FilterActionBar>

          <DataGrid<ILocalityListing>
            columns={[
              { key: 'id', label: 'Código', flex: 0.4 },
              { key: 'description', label: 'Descrição', flex: 2 },
              { key: 'stateId', label: 'Estado Código', flex: 0.8 },
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
        title="Excluir Localidade"
        message={`Deseja realmente excluir a localidade "${deleteTarget?.description}"?`}
        onConfirm={confirmDelete}
        onCancel={closeDeleteConfirm}
        isLoading={isDeleting}
        confirmLabel="Excluir"
      />
    </View>
  );
}
