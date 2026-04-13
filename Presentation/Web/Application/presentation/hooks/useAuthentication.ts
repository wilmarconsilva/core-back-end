import { useCallback, useState } from 'react';
import { AuthenticationService } from '../../application/services/AuthenticationService';
import {
  AccessToken,
  ApiError,
  LoginRequest,
  LoginValidationErrors,
} from '../../application/interfaces/DataTransferObjects/authentication.types';

const authenticationService = new AuthenticationService();

interface AuthenticationState {
  accessToken: AccessToken | null;
  isLoading: boolean;
  error: string | null;
  validationErrors: LoginValidationErrors;
}

interface AuthenticationActions {
  login: (request: LoginRequest) => Promise<void>;
  revoke: () => Promise<void>;
  clearError: () => void;
}

export type UseAuthenticationResult = AuthenticationState & AuthenticationActions;

export function useAuthentication(): UseAuthenticationResult {
  const [accessToken, setAccessToken] = useState<AccessToken | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [validationErrors, setValidationErrors] = useState<LoginValidationErrors>({});

  const login = useCallback(async (request: LoginRequest) => {
    setIsLoading(true);
    setError(null);
    setValidationErrors({});

    try {
      const token = await authenticationService.login(request);
      setAccessToken(token);
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
  }, []);

  const revoke = useCallback(async () => {
    setIsLoading(true);
    try {
      await authenticationService.revoke();
    } finally {
      setAccessToken(null);
      setIsLoading(false);
    }
  }, []);

  const clearError = useCallback(() => {
    setError(null);
    setValidationErrors({});
  }, []);

  return { accessToken, isLoading, error, validationErrors, login, revoke, clearError };
}
