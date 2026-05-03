import React from 'react';
import { FormField } from './FormField';
import { INumberFieldProps } from '../Interfaces/Fields/INumberFieldProps';

export function NumberField({ label, error, ...inputProps }: INumberFieldProps) {
  return (
    <FormField
      label={label}
      error={error}
      keyboardType="numeric"
      {...inputProps}
    />
  );
}
