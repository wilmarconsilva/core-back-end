import { StyleSheet } from 'react-native';
import { colors } from './colors';

export const productsListScreenStyles = StyleSheet.create({
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
