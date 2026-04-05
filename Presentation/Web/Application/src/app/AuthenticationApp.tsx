import React from 'react';
import { createAuthenticationDependencies } from './createAuthenticationDependencies';
import { useAuthenticationViewModel } from '../presentation/authentication/hooks/useAuthenticationViewModel';
import { LoginScreen } from '../presentation/authentication/screens/LoginScreen';

const dependencies = createAuthenticationDependencies();

export function AuthenticationApp() {
  const viewModel = useAuthenticationViewModel(dependencies);

  return <LoginScreen viewModel={viewModel} />;
}
