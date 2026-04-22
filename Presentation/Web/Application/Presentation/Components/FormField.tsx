import React, { ReactNode } from 'react';
import { Text, TextInput, TextInputProps, View } from 'react-native';
import { colors, formStyles } from '../Styles';

interface FormFieldProps extends TextInputProps {
  label: string;
  error?: string;
  children?: ReactNode;
}

export function FormField({ label, error, style, children, ...inputProps }: FormFieldProps) {
  return (
    <View style={formStyles.field}>
      <Text style={formStyles.label}>{label}</Text>
      {children ?? (
        <TextInput
          style={[formStyles.input, error ? formStyles.inputError : null, style]}
          placeholderTextColor={colors.textPlaceholder}
          autoCapitalize="none"
          autoCorrect={false}
          {...inputProps}
        />
      )}
      {error ? <Text style={formStyles.fieldError}>{error}</Text> : null}
    </View>
  );
}
