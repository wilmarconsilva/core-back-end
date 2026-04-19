import { StatusBar } from 'expo-status-bar';
import { Text, View, StyleSheet } from 'react-native';
import Toast from 'react-native-toast-message';
import { LoginScreen } from './Presentation/Screens/LoginScreen';
import { UseAuthentication } from './Application/Hooks/UseAuthentication';

export default function App() {
  const { accessToken, isLoading, error, validationErrors, login, revoke, clearError } =
    UseAuthentication();

  if (accessToken) {
    return (
      <View style={styles.authenticated}>
        <Text style={styles.authenticatedText}>✅ Autenticado com sucesso!</Text>
        <Text style={styles.tokenLabel}>
          Token expira em:{' '}
          {new Date(accessToken.accessTokenExpirationDateTime).toLocaleString('pt-BR')}
        </Text>
        <StatusBar style="auto" />
        <Toast />
      </View>
    );
  }

  return (
    <>
      <StatusBar style="dark" />
      <LoginScreen
        isLoading={isLoading}
        error={error}
        validationErrors={validationErrors}
        onLogin={login}
        onClearError={clearError}
      />
      <Toast />
    </>
  );
}

const styles = StyleSheet.create({
  authenticated: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#F0FDF4',
    padding: 24,
  },
  authenticatedText: {
    fontSize: 22,
    fontWeight: '700',
    color: '#15803D',
    marginBottom: 12,
  },
  tokenLabel: {
    fontSize: 13,
    color: '#6B7280',
  },
});
