import React, { useState } from 'react';
import {
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  Text,
  View,
} from 'react-native';
import { FormField } from '../Components/FormField';
import { PasswordField } from '../Components/PasswordField';
import { PrimaryButton } from '../Components/PrimaryButton';
import { LoginScreenProps } from '../Interfaces/LoginScreenProps';
import { layoutStyles, typographyStyles } from '../Styles';

export function LoginScreen({
  isLoading,
  validationErrors,
  onLogin,
}: LoginScreenProps) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = () => {
    onLogin({ email: email, password });
  };

  return (
    <KeyboardAvoidingView
      style={layoutStyles.screenWrapper}
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
    >
      <ScrollView
        contentContainerStyle={layoutStyles.scrollContainer}
        keyboardShouldPersistTaps="handled"
      >
        <View style={layoutStyles.card}>
          <Text style={typographyStyles.title}>Bem-vindo</Text>
          <Text style={typographyStyles.subtitle}>Faça login para continuar</Text>

          <FormField
            label="E-mail"
            placeholder="seu@email.com"
            keyboardType="email-address"
            value={email}
            onChangeText={setEmail}
            editable={!isLoading}
            error={validationErrors.email}
          />

          <PasswordField
            label="Senha"
            placeholder="••••••••"
            value={password}
            onChangeText={setPassword}
            editable={!isLoading}
            error={validationErrors.password}
          />

          <PrimaryButton
            title="Entrar"
            isLoading={isLoading}
            onPress={handleLogin}
          />
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

