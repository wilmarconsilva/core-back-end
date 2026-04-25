import React from 'react';
import {
  ActivityIndicator,
  Text,
  TouchableOpacity,
  TouchableOpacityProps,
} from 'react-native';
import { buttonStyles, colors } from '../Styles';

interface SecondaryButtonProps extends TouchableOpacityProps {
  title: string;
  isLoading?: boolean;
}

export function SecondaryButton({ title, isLoading, disabled, style, ...props }: SecondaryButtonProps) {
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
