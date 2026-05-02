import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { FormField } from '../../Components/FormField';
import { UseCustomers } from '../../../Application/Hooks/UseCustomers';
import { ICustomerListing } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { ICustomersListScreenProps } from '../../Interfaces/Customer/ICustomersListScreenProps';
import { customersListScreenStyles as styles, layoutStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

export function CustomersListScreen({ onGoBack, onNavigateToForm }: ICustomersListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove } = UseCustomers();
  const [page, setPage] = useState(1);
  const [name, setName] = useState('');
  const [deleteTarget, setDeleteTarget] = useState<ICustomerListing | null>(null);

  const buildFilter = useCallback((p: number): ICustomerListingFilter => ({
    registersStart: (p - 1) * PAGE_SIZE,
    registersEnd: p * PAGE_SIZE,
    name: name.trim() || undefined,
  }), [name]);

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
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb items={[{ label: 'Início', onPress: onGoBack }, { label: 'Clientes' }]} />
        <Text style={typographyStyles.title}>Clientes</Text>

        <View style={styles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()}>
            <View style={styles.filterField}>
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
              { key: 'document', label: 'Documento', flex: 1.2 },
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
        title="Excluir Cliente"
        message={`Deseja excluir o cliente "${deleteTarget?.name}"?`}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(null)}
        isLoading={isDeleting}
        confirmLabel="Excluir"
      />
    </View>
  );
}
