import { StyleSheet } from 'react-native';
import { colors } from './colors';

export const tableStyles = StyleSheet.create({
  container: {
    backgroundColor: colors.surface,
    borderRadius: 8,
    overflow: 'hidden',
    marginTop: 8,
  },
  header: {
    flexDirection: 'row',
    backgroundColor: colors.primary,
    paddingVertical: 10,
    paddingHorizontal: 8,
  },
  headerCell: {
    color: colors.textWhite,
    fontWeight: '600',
    fontSize: 13,
  },
  row: {
    flexDirection: 'row',
    paddingVertical: 10,
    paddingHorizontal: 8,
    borderBottomWidth: 1,
    borderBottomColor: colors.border,
    alignItems: 'center',
  },
  rowAlt: {
    backgroundColor: colors.surfaceAlt,
  },
  cell: {
    fontSize: 13,
    color: colors.textPrimary,
  },
  actionsCell: {
    flexDirection: 'row',
    gap: 8,
  },
  paginationContainer: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    paddingVertical: 12,
    gap: 16,
  },
  paginationText: {
    fontSize: 13,
    color: colors.textSecondary,
  },
  emptyText: {
    textAlign: 'center',
    color: colors.textSecondary,
    paddingVertical: 24,
    fontSize: 14,
  },
});
