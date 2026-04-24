import { useCallback, useEffect, useState } from 'react';
import { AuthenticationService } from '../Services/AuthenticationService';
import { TokenStorage } from '../Services/TokenStorage';
import { AccessToken } from '../Interfaces/DataTransferObjects/AccessToken';
import { ApiError } from '../Interfaces/DataTransferObjects/ApiError';
import { LoginRequest } from '../Interfaces/DataTransferObjects/LoginRequest';
import { LoginValidationErrors } from '../Interfaces/DataTransferObjects/LoginValidationErrors';

const authenticationService = new AuthenticationService();

interface AuthenticationState {
  accessToken: AccessToken | null;
  isLoading: boolean;
  isInitializing: boolean;
  error: string | null;
  validationErrors: LoginValidationErrors;
}

interface AuthenticationActions {
  login: (request: LoginRequest) => Promise<void>;
  clearError: () => void;
}

export type UseAuthenticationResult = AuthenticationState & AuthenticationActions;

export function UseAuthentication(): UseAuthenticationResult {
  const [accessToken, setAccessToken] = useState<AccessToken | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isInitializing, setIsInitializing] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [validationErrors, setValidationErrors] = useState<LoginValidationErrors>({});

  const storeToken = useCallback((token: AccessToken) => {
    TokenStorage.setToken(token.accessToken);
    setAccessToken(token);
  }, []);

  // Register a global logout callback so the HTTP client can redirect to login
  // when a token refresh fails (e.g. session fully expired).
  useEffect(() => {
    TokenStorage.registerLogoutCallback(() => {
      setAccessToken(null);
    });
  }, []);

  useEffect(() => {
    authenticationService.refresh()
      .then(storeToken)
      .catch(() => {})
      .finally(() => setIsInitializing(false));
  }, [storeToken]);

  const login = useCallback(async (request: LoginRequest) => {
    setIsLoading(true);
    setError(null);
    setValidationErrors({});

    try {
      const token = await authenticationService.login(request);
      storeToken(token);
    } catch (err: unknown) {
      const apiError = err as ApiError;

      if (apiError.status === 400 && apiError.errors) {
        const mapped: LoginValidationErrors = {};
        if (apiError.errors['Email']?.[0]) mapped.email = apiError.errors['Email'][0];
        if (apiError.errors['Password']?.[0]) mapped.password = apiError.errors['Password'][0];
        setValidationErrors(mapped);
      } else {
        setError(apiError.detail ?? apiError.title ?? 'Ocorreu um erro inesperado.');
      }
    } finally {
      setIsLoading(false);
    }
  }, [storeToken]);

  const clearError = useCallback(() => {
    setError(null);
    setValidationErrors({});
  }, []);

  return { accessToken, isLoading, isInitializing, error, validationErrors, login, clearError };
}
