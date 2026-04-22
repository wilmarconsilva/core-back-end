import { Ionicons } from '@expo/vector-icons';
import React, { useState } from 'react';
import {
  TextInput,
  TextInputProps,
  TouchableOpacity,
  View,
} from 'react-native';
import { colors, formStyles } from '../Styles';
import { FormField } from './FormField';

interface PasswordFieldProps extends Omit<TextInputProps, 'secureTextEntry'> {
  label: string;
  error?: string;
}

export function PasswordField({ label, error, ...inputProps }: PasswordFieldProps) {
  const [isVisible, setIsVisible] = useState(false);

  return (
    <FormField label={label} error={error}>
      <View style={[formStyles.passwordContainer, error ? formStyles.inputError : null]}>
        <TextInput
          style={formStyles.passwordInput}
          placeholderTextColor={colors.textPlaceholder}
          secureTextEntry={!isVisible}
          autoCapitalize="none"
          autoCorrect={false}
          {...inputProps}
        />
        <TouchableOpacity
          onPress={() => setIsVisible(!isVisible)}
          style={formStyles.eyeButton}
        >
          <Ionicons
            name={isVisible ? 'eye-off-outline' : 'eye-outline'}
            size={20}
            color={colors.textSecondary}
          />
        </TouchableOpacity>
      </View>
    </FormField>
  );
}
