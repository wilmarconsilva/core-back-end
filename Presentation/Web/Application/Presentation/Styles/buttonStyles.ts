import { StyleSheet } from 'react-native';
import { colors } from './colors';

export const buttonStyles = StyleSheet.create({
  primary: {
    backgroundColor: colors.primary,
    borderRadius: 8,
    paddingVertical: 14,
    alignItems: 'center',
    marginTop: 8,
  },
  primaryDisabled: {
    backgroundColor: colors.primaryLight,
  },
  primaryText: {
    color: colors.textWhite,
    fontSize: 16,
    fontWeight: '600',
  },
});
