import React from 'react';
import { TextInputProps } from 'react-native';
import { FormField } from './FormField';

interface NumberFieldProps extends Omit<TextInputProps, 'keyboardType'> {
  label: string;
  error?: string;
}

export function NumberField({ label, error, ...inputProps }: NumberFieldProps) {
  return (
    <FormField
      label={label}
      error={error}
      keyboardType="numeric"
      {...inputProps}
    />
  );
}
