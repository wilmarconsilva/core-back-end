import React from 'react';
import { TextInputProps } from 'react-native';
import { FormField } from './FormField';

interface TextAreaFieldProps extends Omit<TextInputProps, 'multiline' | 'numberOfLines'> {
  label: string;
  error?: string;
  numberOfLines?: number;
}

export function TextAreaField({ label, error, numberOfLines = 4, style, ...inputProps }: TextAreaFieldProps) {
  return (
    <FormField
      label={label}
      error={error}
      multiline
      numberOfLines={numberOfLines}
      style={[{ height: numberOfLines * 24, textAlignVertical: 'top' }, style]}
      {...inputProps}
    />
  );
}
