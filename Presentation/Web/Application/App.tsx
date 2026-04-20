import { StatusBar } from 'expo-status-bar';
import { LoginScreen } from './Presentation/Screens/LoginScreen';
import { HomeScreen } from './Presentation/Screens/HomeScreen';
import { AppToast } from './Presentation/Components/AppToast';
import { UseAuthentication } from './Application/Hooks/UseAuthentication';

export default function App() {
  const { accessToken, isLoading, error, validationErrors, login, revoke, clearError } =
    UseAuthentication();

  if (accessToken) {
    return (
      <>
        <StatusBar style="dark" />
        <HomeScreen />
        <AppToast />
      </>
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
      <AppToast />
    </>
  );
}
