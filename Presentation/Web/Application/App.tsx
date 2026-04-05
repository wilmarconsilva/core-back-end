import React from 'react';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { AuthenticationApp } from './src/app/AuthenticationApp';

export default function App() {
  return (
    <SafeAreaProvider>
      <AuthenticationApp />
    </SafeAreaProvider>
  );
}
