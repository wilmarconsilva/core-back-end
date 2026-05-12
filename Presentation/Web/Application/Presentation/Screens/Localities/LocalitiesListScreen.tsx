import React, { useCallback, useState } from 'react';
import { View } from 'react-native';
import { FormField } from '../../Components/FormField';
import { UsePagedCrudList } from '../../../Application/Hooks/UsePagedCrudList';
import { UseLocalities } from '../../../Application/Hooks/UseLocalities';
import { ILocalityListing } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';
import { ICrudListScreenProps } from '../../Interfaces/Screens/ICrudListScreenProps';
import { listScreenStyles } from '../../Styles';
import { CrudListScreen } from '../CrudListScreen';

export function LocalitiesListScreen({ onGoBack, onNavigateToForm }: ICrudListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove } = UseLocalities();
  const [description, setDescription] = useState('');

  const buildFilter = useCallback((p: number, pageSize: number): ILocalityListingFilter => ({
    registersStart: (p - 1) * pageSize,
    registersEnd: p * pageSize,
    description: description.trim() || undefined,
  }), [description]);

  const listState = UsePagedCrudList<ILocalityListing, ILocalityListingFilter>({
    loadAll,
    remove,
    buildFilter,
  });

  return (
    <CrudListScreen<ILocalityListing>
      title="Localidades"
      onGoBack={onGoBack}
      onNavigateToForm={onNavigateToForm}
      filterContent={(
        <View style={listScreenStyles.filterFieldFlush}>
          <FormField
            label="Descrição"
            placeholder="Filtrar por descrição..."
            value={description}
            onChangeText={setDescription}
            onSubmitEditing={listState.handleSearch}
          />
        </View>
      )}
      columns={[
        { key: 'id', label: 'Código', flex: 0.4 },
        { key: 'description', label: 'Descrição', flex: 2 },
        { key: 'stateId', label: 'Estado Código', flex: 0.8 },
      ]}
      data={listings}
      isLoading={isLoadingList}
      isDeleting={isDeleting}
      page={listState.page}
      pageSize={listState.pageSize}
      onSearch={listState.handleSearch}
      onPageChange={listState.handlePageChange}
      onDelete={listState.openDeleteConfirm}
      deleteDialogVisible={listState.deleteDialogVisible}
      deleteTarget={listState.deleteTarget}
      deleteTitle="Excluir Localidade"
      getDeleteMessage={(item) => `Deseja realmente excluir a localidade "${item?.description}"?`}
      onConfirmDelete={listState.confirmDelete}
      onCancelDelete={listState.closeDeleteConfirm}
      newLabel="+ Nova"
    />
  );
}
