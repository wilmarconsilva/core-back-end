import React from 'react';
import { Platform, TextInput, TextInputProps } from 'react-native';
import { FormField } from './FormField';
import { colors, formStyles } from '../Styles';

interface DateFieldProps extends Omit<TextInputProps, 'keyboardType'> {
  label: string;
  error?: string;
}

export function DateField({ label, error, style, ...inputProps }: DateFieldProps) {
  if (Platform.OS === 'web') {
    return (
      <FormField label={label} error={error}>
        {/* @ts-ignore */}
        <input
          type="date"
          value={inputProps.value as string ?? ''}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) => inputProps.onChangeText?.(e.target.value)}
          style={{
            borderWidth: 1,
            borderColor: error ? colors.error : colors.border,
            borderRadius: 8,
            paddingLeft: 14,
            paddingRight: 14,
            paddingTop: 12,
            paddingBottom: 12,
            fontSize: 15,
            color: colors.textPrimary,
            backgroundColor: colors.surfaceAlt,
            width: '100%',
            boxSizing: 'border-box',
            outline: 'none',
            fontFamily: 'inherit',
          } as React.CSSProperties}
          disabled={inputProps.editable === false}
        />
      </FormField>
    );
  }

  return (
    <FormField
      label={label}
      error={error}
      placeholder="AAAA-MM-DD"
      style={style}
      {...inputProps}
    />
  );
}
