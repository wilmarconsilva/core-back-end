import React, { ReactNode } from 'react';
import { StyleSheet, View } from 'react-native';
import { PrimaryButton } from './PrimaryButton';

interface FilterActionBarProps {
  /** Filter fields to render on the left side */
  children: ReactNode;
  onSearch: () => void;
  onNew?: () => void;
  newLabel?: string;
  isSearching?: boolean;
}

/**
 * Standardized filter row used on all list screens.
 * Places filter fields on the left and action buttons (Search / New) on the right,
 * all vertically aligned at the bottom edge.
 */
export function FilterActionBar({
  children,
  onSearch,
  onNew,
  newLabel = '+ Novo',
  isSearching,
}: FilterActionBarProps) {
  return (
    <View style={styles.container}>
      <View style={styles.filters}>{children}</View>
      <View style={styles.actions}>
        <PrimaryButton
          title="Buscar"
          onPress={onSearch}
          isLoading={isSearching}
          style={styles.btn}
        />
        {onNew && (
          <PrimaryButton
            title={newLabel}
            onPress={onNew}
            style={styles.btn}
          />
        )}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    gap: 8,
    marginBottom: 12,
    alignItems: 'center',
    flexWrap: 'wrap',
  },
  filters: {
    flex: 1,
    flexDirection: 'row',
    gap: 8,
    flexWrap: 'wrap',
  },
  actions: {
    flexDirection: 'row',
    gap: 8,
  },
  btn: {
    marginTop: 0,
    paddingHorizontal: 16,
    paddingVertical: 12,
  },
});
