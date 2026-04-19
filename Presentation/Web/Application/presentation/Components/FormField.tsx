import React from 'react';
import { Text, TextInput, TextInputProps, View } from 'react-native';
import { colors, formStyles } from '../Styles';

interface FormFieldProps extends TextInputProps {
  label: string;
  error?: string;
}

export function FormField({ label, error, style, ...inputProps }: FormFieldProps) {
  return (
    <View style={formStyles.field}>
      <Text style={formStyles.label}>{label}</Text>
      <TextInput
        style={[formStyles.input, error ? formStyles.inputError : null, style]}
        placeholderTextColor={colors.textPlaceholder}
        autoCapitalize="none"
        autoCorrect={false}
        {...inputProps}
      />
      {error ? <Text style={formStyles.fieldError}>{error}</Text> : null}
    </View>
  );
}
