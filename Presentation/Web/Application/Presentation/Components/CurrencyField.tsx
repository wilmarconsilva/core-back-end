import React from 'react';
import { Masks } from 'react-native-mask-input';
import { FormField } from './FormField';
import { ICurrencyFieldProps } from '../Interfaces/Fields/ICurrencyFieldProps';

export function CurrencyField({ label, error, ...inputProps }: ICurrencyFieldProps) {
  return (
    <FormField
      label={label}
      error={error}
      keyboardType="numeric"
      mask={Masks.BRL_CURRENCY}
      {...inputProps}
    />
  );
}
