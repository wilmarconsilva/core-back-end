import React, { useCallback, useEffect, useState } from 'react';
import { ScrollView, StyleSheet, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { ConfirmDialog } from '../../Components/ConfirmDialog';
import { DataGrid } from '../../Components/DataGrid';
import { FilterActionBar } from '../../Components/FilterActionBar';
import { FormField } from '../../Components/FormField';
import { UseLocalities } from '../../../Application/Hooks/UseLocalities';
import { ILocalityListing } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';
import { colors, layoutStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

interface LocalitiesListScreenProps {
  onGoBack: () => void;
  onNavigateToForm: (id?: number) => void;
}

const styles = StyleSheet.create({
  filterField: { flex: 1, marginBottom: 0 },
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

export function LocalitiesListScreen({ onGoBack, onNavigateToForm }: LocalitiesListScreenProps) {
  const { listings, isLoadingList, isDeleting, loadAll, remove } = UseLocalities();
  const [page, setPage] = useState(1);
  const [description, setDescription] = useState('');
  const [deleteTarget, setDeleteTarget] = useState<ILocalityListing | null>(null);

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

  const handleDelete = async () => {
    if (!deleteTarget) return;
    const success = await remove(deleteTarget.id);
    if (success) {
      setDeleteTarget(null);
      loadAll(buildFilter(page));
    }
  };

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb
          items={[{ label: 'Início', onPress: onGoBack }, { label: 'Localidades' }]}
        />
        <Text style={typographyStyles.title}>Localidades</Text>

        <View style={styles.sectionCard}>
          <FilterActionBar onSearch={handleSearch} onNew={() => onNavigateToForm()} newLabel="+ Nova">
            <View style={styles.filterField}>
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
            onDelete={(item) => setDeleteTarget(item)}
            keyExtractor={(item) => String(item.id)}
          />
        </View>
      </ScrollView>

      <ConfirmDialog
        visible={!!deleteTarget}
        title="Excluir Localidade"
        message={`Deseja realmente excluir a localidade "${deleteTarget?.description}"?`}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(null)}
        isLoading={isDeleting}
        confirmLabel="Excluir"
      />
    </View>
  );
}
