import { useCallback, useEffect, useState } from 'react';
import { AuthenticationService } from '../Services/AuthenticationService';
import { TokenStorageService } from '../Services/TokenStorageService';
import { registerRefreshFn } from '../Services/HttpClient';
import { IAccessToken } from '../Interfaces/DataTransferObjects/Authentication/IAccessToken';
import { IApiError } from '../Interfaces/DataTransferObjects/Authentication/IApiError';
import { ILoginRequest } from '../Interfaces/DataTransferObjects/Authentication/ILoginRequest';
import { ILoginValidationErrors } from '../Interfaces/DataTransferObjects/Authentication/ILoginValidationErrors';

const authenticationService = new AuthenticationService();

interface AuthenticationState {
  accessToken: IAccessToken | null;
  isLoading: boolean;
  isInitializing: boolean;
  error: string | null;
  validationErrors: ILoginValidationErrors;
}

interface AuthenticationActions {
  login: (request: ILoginRequest) => Promise<void>;
  clearError: () => void;
}

export type UseAuthenticationResult = AuthenticationState & AuthenticationActions;

export function UseAuthentication(): UseAuthenticationResult {
  const [accessToken, setAccessToken] = useState<IAccessToken | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isInitializing, setIsInitializing] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [validationErrors, setValidationErrors] = useState<ILoginValidationErrors>({});

  const storeToken = useCallback((token: IAccessToken) => {
    TokenStorageService.setToken(token);
    setAccessToken(token);
  }, []);

  useEffect(() => {
    // Inject the refresh function into the HTTP client to avoid a circular import
    // (AuthenticationService → APIConsumerService → HttpClient → AuthenticationService).
    registerRefreshFn(() => authenticationService.refresh());

    // Keep React state in sync when the HTTP interceptor silently refreshes the token.
    TokenStorageService.registerTokenRefreshedCallback(setAccessToken);

    // Clear React state when the HTTP interceptor triggers logout after a failed refresh.
    TokenStorageService.registerLogoutCallback(() => setAccessToken(null));
  }, []);

  useEffect(() => {
    authenticationService.refresh()
      .then(storeToken)
      .catch(() => {})
      .finally(() => setIsInitializing(false));
  }, [storeToken]);

  const login = useCallback(async (request: ILoginRequest) => {
    setIsLoading(true);
    setError(null);
    setValidationErrors({});

    try {
      const token = await authenticationService.login(request);
      storeToken(token);
    } catch (err: unknown) {
      const apiError = err as IApiError;

      if (apiError.status === 400 && apiError.errors) {
        const mapped: ILoginValidationErrors = {};
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
