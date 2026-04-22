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
  return (
    <TouchableOpacity
      style={[buttonStyles.primary, (isLoading || disabled) ? buttonStyles.primaryDisabled : null, style]}
      disabled={isLoading || disabled}
      activeOpacity={0.8}
      {...props}
    >
      {isLoading ? (
        <ActivityIndicator color={colors.textWhite} />
      ) : (
        <Text style={buttonStyles.primaryText}>{title}</Text>
      )}
    </TouchableOpacity>
  );
}
