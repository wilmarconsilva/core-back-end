import React from 'react';
import {
  ActivityIndicator,
  StyleSheet,
  Text,
  TouchableOpacity,
  TouchableOpacityProps,
} from 'react-native';
import { colors } from '../Styles';

interface SecondaryButtonProps extends TouchableOpacityProps {
  title: string;
  isLoading?: boolean;
}

const styles = StyleSheet.create({
  button: {
    borderWidth: 1.5,
    borderColor: colors.primary,
    borderRadius: 8,
    paddingVertical: 12,
    alignItems: 'center',
    marginTop: 8,
    backgroundColor: 'transparent',
  },
  buttonDisabled: {
    borderColor: colors.primaryLight,
  },
  text: {
    color: colors.primary,
    fontSize: 15,
    fontWeight: '600',
  },
  textDisabled: {
    color: colors.primaryLight,
  },
});

export function SecondaryButton({ title, isLoading, disabled, style, ...props }: SecondaryButtonProps) {
  const isDisabled = isLoading || disabled;
  return (
    <TouchableOpacity
      style={[styles.button, isDisabled ? styles.buttonDisabled : null, style]}
      disabled={isDisabled}
      activeOpacity={0.7}
      {...props}
    >
      {isLoading ? (
        <ActivityIndicator color={colors.primary} />
      ) : (
        <Text style={[styles.text, isDisabled ? styles.textDisabled : null]}>{title}</Text>
      )}
    </TouchableOpacity>
  );
}
