import { useCallback, useEffect, useState } from 'react';
import { AuthenticationService } from '../Services/AuthenticationService';
import { TokenStorageService } from '../Services/TokenStorageService';
import { registerRefreshFn } from '../Services/HttpClientService';
import { IAccessToken } from '../Interfaces/DataTransferObjects/Authentication/IAccessToken';
import { ILoginRequest } from '../Interfaces/DataTransferObjects/Authentication/ILoginRequest';

const authenticationService = new AuthenticationService();

interface AuthenticationState {
  accessToken: IAccessToken | null;
  isLoading: boolean;
  isInitializing: boolean;
}

interface AuthenticationActions {
  login: (request: ILoginRequest) => Promise<void>;
}

export type UseAuthenticationResult = AuthenticationState & AuthenticationActions;

export function UseAuthentication(): UseAuthenticationResult {
  const [accessToken, setAccessToken] = useState<IAccessToken | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isInitializing, setIsInitializing] = useState(true);

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

    try 
    {
      const token = await authenticationService.login(request);
      storeToken(token);
    }
    catch
    {
      // Error already shown by APIConsumerService
    }
    finally 
    {
      setIsLoading(false);
    }
  }, [storeToken]);

  return { accessToken, isLoading, isInitializing, login };
}
