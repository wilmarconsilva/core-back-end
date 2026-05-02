import React from 'react';
import {
  ActivityIndicator,
  Text,
  TouchableOpacity,
} from 'react-native';
import { ISecondaryButtonProps } from '../Interfaces/Button/ISecondaryButtonProps';
import { buttonStyles, colors } from '../Styles';

export function SecondaryButton({ title, isLoading, disabled, style, ...props }: ISecondaryButtonProps) {
  const isDisabled = isLoading || disabled;
  return (
    <TouchableOpacity
      style={[buttonStyles.base, buttonStyles.secondary, isDisabled && buttonStyles.disabled, style]}
      disabled={isDisabled}
      activeOpacity={0.7}
      {...props}
    >
      {isLoading ? (
        <ActivityIndicator color={colors.textPrimary} />
      ) : (
        <Text style={[buttonStyles.baseText, buttonStyles.secondaryText]}>{title}</Text>
      )}
    </TouchableOpacity>
  );
}
