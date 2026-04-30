import React, { ReactNode, useEffect, useState } from 'react';
import {
  Modal,
  ScrollView,
  StyleSheet,
  Text,
  TouchableOpacity,
  View,
} from 'react-native';
import { colors, formStyles } from '../Styles';
import { FilterActionBar } from './FilterActionBar';
import { DataGrid } from './DataGrid';

interface DataGridColumn {
  key: string;
  label: string;
  flex?: number;
}

interface EntitySelectFieldProps<TListing, TFilter> {
  label: string;
  value?: number | string;
  placeholder?: string;
  error?: string;
  disabled?: boolean;

  listings: TListing[];
  isLoading: boolean;
  onLoadData: (filter: TFilter) => Promise<void>;

  pageSize: number;
  buildFilter: (page: number, filterValues: Record<string, any>) => TFilter;

  columns: DataGridColumn[];
  keyExtractor: (item: TListing) => string;
  labelExtractor: (item: TListing) => string;

  onValueChange: (item: TListing) => void;
  children?: ReactNode;
}

const styles = StyleSheet.create({
  trigger: {
    borderWidth: 1,
    borderColor: colors.border,
    borderRadius: 8,
    paddingHorizontal: 14,
    paddingVertical: 12,
    backgroundColor: colors.surfaceAlt,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  triggerError: {
    borderColor: colors.error,
  },
  triggerText: {
    fontSize: 15,
    color: colors.textPrimary,
    flex: 1,
  },
  placeholder: {
    color: colors.textPlaceholder,
  },
  arrow: {
    fontSize: 12,
    color: colors.textSecondary,
    marginLeft: 8,
  },
  overlay: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.4)',
    justifyContent: 'center',
    alignItems: 'center',
    padding: 16,
  },
  modal: {
    backgroundColor: colors.surface,
    borderRadius: 12,
    width: '100%',
    maxHeight: '90%',
    shadowColor: colors.shadow,
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.15,
    shadowRadius: 12,
    elevation: 8,
  },
  header: {
    paddingHorizontal: 16,
    paddingVertical: 14,
    borderBottomWidth: 1,
    borderBottomColor: colors.border,
  },
  headerText: {
    fontSize: 15,
    fontWeight: '600',
    color: colors.textPrimary,
  },
  filterSection: {
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: colors.border,
  },
  filterField: {
    flex: 1,
    minWidth: 180,
  },
  contentContainer: {
    flex: 1,
  },
  footer: {
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderTopWidth: 1,
    borderTopColor: colors.border,
    alignItems: 'center',
  },
  cancelButton: {
    paddingVertical: 10,
    paddingHorizontal: 16,
  },
  cancelText: {
    fontSize: 15,
    color: colors.textSecondary,
  },
});

export function EntitySelectField<TListing, TFilter>({
  label,
  value,
  placeholder = 'Selecione...',
  error,
  disabled,
  listings,
  isLoading,
  onLoadData,
  pageSize,
  buildFilter,
  columns,
  keyExtractor,
  labelExtractor,
  onValueChange,
  children,
}: EntitySelectFieldProps<TListing, TFilter>) {
  const [isOpen, setIsOpen] = useState(false);
  const [page, setPage] = useState(1);
  const [filterValues, setFilterValues] = useState<Record<string, any>>({});

  useEffect(() => {
    if (isOpen) {
      const filter = buildFilter(1, filterValues);
      onLoadData(filter);
    }
  }, [isOpen]);

  const selectedItem = listings.find(item => keyExtractor(item) === String(value));

  const handleSearch = async () => {
    setPage(1);
    const filter = buildFilter(1, filterValues);
    await onLoadData(filter);
  };

  const handlePageChange = (p: number) => {
    setPage(p);
    const filter = buildFilter(p, filterValues);
    onLoadData(filter);
  };

  const handleSelectItem = (item: TListing) => {
    onValueChange(item);
    setIsOpen(false);
  };

  return (
    <View style={formStyles.field}>
      <Text style={formStyles.label}>{label}</Text>
      <TouchableOpacity
        style={[styles.trigger, error ? styles.triggerError : null]}
        onPress={() => !disabled && setIsOpen(true)}
        activeOpacity={0.7}
        disabled={disabled}
      >
        <Text
          style={[styles.triggerText, !selectedItem ? styles.placeholder : null]}
          numberOfLines={1}
        >
          {selectedItem ? labelExtractor(selectedItem) : placeholder}
        </Text>
        <Text style={styles.arrow}>▼</Text>
      </TouchableOpacity>
      {error ? <Text style={formStyles.fieldError}>{error}</Text> : null}

      <Modal
        visible={isOpen}
        transparent
        animationType="fade"
        onRequestClose={() => setIsOpen(false)}
      >
        <TouchableOpacity
          style={styles.overlay}
          activeOpacity={1}
          onPress={() => setIsOpen(false)}
        >
          <View style={styles.modal}>
            <View style={styles.header}>
              <Text style={styles.headerText}>{label}</Text>
            </View>

            <View style={styles.filterSection}>
              <FilterActionBar onSearch={handleSearch}>
                {children && (
                  <View style={styles.filterField}>
                    {children}
                  </View>
                )}
              </FilterActionBar>
            </View>

            <ScrollView style={styles.contentContainer} keyboardShouldPersistTaps="handled">
              <DataGrid<TListing>
                columns={columns}
                data={listings}
                isLoading={isLoading}
                page={page}
                pageSize={pageSize}
                onPageChange={handlePageChange}
                onEdit={(item) => handleSelectItem(item)}
                keyExtractor={keyExtractor}
              />
            </ScrollView>

            <View style={styles.footer}>
              <TouchableOpacity style={styles.cancelButton} onPress={() => setIsOpen(false)}>
                <Text style={styles.cancelText}>Cancelar</Text>
              </TouchableOpacity>
            </View>
          </View>
        </TouchableOpacity>
      </Modal>
    </View>
  );
}
