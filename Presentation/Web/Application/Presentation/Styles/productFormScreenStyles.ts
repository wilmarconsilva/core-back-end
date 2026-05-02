import { StyleSheet } from 'react-native';
import { colors } from './colors';

export const productFormScreenStyles = StyleSheet.create({
  card: {
    backgroundColor: colors.surface,
    borderRadius: 12,
    padding: 24,
    shadowColor: colors.shadow,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.06,
    shadowRadius: 4,
    elevation: 2,
  },
  actionRow: { flexDirection: 'row', gap: 12, marginTop: 8 },
});
