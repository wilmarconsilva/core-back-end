import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { FormField } from '../../Components/FormField';
import { UseCustomers } from '../../../Application/Hooks/UseCustomers';
import { UseDeleteConfirmDialog } from '../../../Application/Hooks/UseDeleteConfirmDialog';
import { ICustomerListing } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { ICrudListScreenProps } from '../../Interfaces/Screens/ICrudListScreenProps';
import { layoutStyles, listScreenStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

export function CustomersListScreen({ onGoBack, onNavigateToForm }: ICrudListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove } = UseCustomers();
  const [page, setPage] = useState(1);
  const [name, setName] = useState('');

  const buildFilter = useCallback((p: number): ICustomerListingFilter => ({
    registersStart: (p - 1) * PAGE_SIZE,
    registersEnd: p * PAGE_SIZE,
    name: name.trim() || undefined,
  }), [name]);

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
  } = UseDeleteConfirmDialog<ICustomerListing>({ remove, reloadAfterDelete: reloadCurrentPage });

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb items={[{ label: 'Início', onPress: onGoBack }, { label: 'Clientes' }]} />
        <Text style={typographyStyles.title}>Clientes</Text>

        <View style={layoutStyles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()}>
            <View style={listScreenStyles.filterFieldWide}>
              <FormField
                label="Nome"
                placeholder="Filtrar por nome..."
                value={name}
                onChangeText={setName}
                onSubmitEditing={handleSearch}
              />
            </View>
          </FilterActionBar>

          <DataGrid<ICustomerListing>
            columns={[
              { key: 'id', label: 'Código', flex: 0.4 },
              { key: 'name', label: 'Nome', flex: 2 },
              { key: 'localityState', label: 'Localidade/Estado', flex: 1.2 },
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
        title="Excluir Cliente"
        message={`Deseja excluir o cliente "${deleteTarget?.name}"?`}
        onConfirm={confirmDelete}
        onCancel={closeDeleteConfirm}
        isLoading={isDeleting}
        confirmLabel="Excluir"
      />
    </View>
  );
}
