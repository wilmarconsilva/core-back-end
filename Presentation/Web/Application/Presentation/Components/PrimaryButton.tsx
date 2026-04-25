import React from 'react';
import {
  ActivityIndicator,
  Text,
  TouchableOpacity,
  TouchableOpacityProps,
} from 'react-native';
import { buttonStyles, colors } from '../Styles';

interface PrimaryButtonProps extends TouchableOpacityProps {
  title: string;
  isLoading?: boolean;
}

export function PrimaryButton({ title, isLoading, disabled, style, ...props }: PrimaryButtonProps) {
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
