import React from 'react';
import {
  ActivityIndicator,
  Text,
  TouchableOpacity,
} from 'react-native';
import { IPrimaryButtonProps } from '../Interfaces/Button/IPrimaryButtonProps';
import { buttonStyles, colors } from '../Styles';

export function PrimaryButton({ title, isLoading, disabled, style, ...props }: IPrimaryButtonProps) {
  const isDisabled = isLoading || disabled;
  return (
    <TouchableOpacity
      style={[buttonStyles.base, buttonStyles.primary, isDisabled && buttonStyles.disabled, style]}
      disabled={isDisabled}
      activeOpacity={0.8}
      {...props}
    >
      {isLoading ? (
        <ActivityIndicator color={colors.textWhite} />
      ) : (
        <Text style={[buttonStyles.baseText, buttonStyles.primaryText]}>{title}</Text>
      )}
    </TouchableOpacity>
  );
}
