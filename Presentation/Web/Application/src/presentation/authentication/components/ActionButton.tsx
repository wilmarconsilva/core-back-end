import React from 'react';
import { Pressable, Text } from 'react-native';
import { styles } from '../styles';

type ActionButtonProps = {
  disabled?: boolean;
  label: string;
  onPress: () => void;
  primary?: boolean;
};

export function ActionButton({ disabled, label, onPress, primary = false }: ActionButtonProps) {
  return (
    <Pressable
      disabled={disabled}
      onPress={onPress}
      style={({ pressed }) => [
        styles.button,
        primary ? styles.buttonPrimary : styles.buttonSecondary,
        (disabled || pressed) && styles.buttonDisabled,
      ]}
    >
      <Text style={[styles.buttonText, primary ? styles.buttonTextPrimary : styles.buttonTextSecondary]}>{label}</Text>
    </Pressable>
  );
}
