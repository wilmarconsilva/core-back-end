import React from 'react';
import { Text, TextInput, View } from 'react-native';
import { IFormFieldProps } from '../Interfaces/Fields/IFormFieldProps';
import { colors, formStyles } from '../Styles';

export function FormField({ label, error, style, children, ...inputProps }: IFormFieldProps) {
  const { onSubmitEditing, returnKeyType, ...restInput } = inputProps;
  return (
    <View style={formStyles.field}>
      <Text style={formStyles.label}>{label}</Text>
      {children ?? (
        <TextInput
          style={[formStyles.input, error ? formStyles.inputError : null, style]}
          placeholderTextColor={colors.textPlaceholder}
          autoCapitalize="none"
          autoCorrect={false}
          returnKeyType={returnKeyType ?? (onSubmitEditing ? 'search' : undefined)}
          onSubmitEditing={onSubmitEditing}
          {...restInput}
        />
      )}
      {error ? <Text style={formStyles.fieldError}>{error}</Text> : null}
    </View>
  );
}
