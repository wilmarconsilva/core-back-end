import Toast from 'react-native-toast-message';
import { IAccessToken } from '../Interfaces/DataTransferObjects/Authentication/IAccessToken';
import { TokenStorageService } from './TokenStorageService';

if (!process.env?.EXPO_PUBLIC_API_BASE_URL?.trim()) {
  throw new Error('EXPO_PUBLIC_API_BASE_URL is not defined in the .env file.');
}

export const API_BASE_URL = process.env.EXPO_PUBLIC_API_BASE_URL as string;

export class SessionExpiredError extends Error {
  constructor() {
    super('Session expired');
    this.name = 'SessionExpiredError';
  }
}

type RefreshFn = () => Promise<IAccessToken>;

type PendingEntry = {
  retryFn: () => Promise<Response>;
  resolve: (value: Response) => void;
  reject: (reason: unknown) => void;
};

let _refreshFn: RefreshFn | null = null;
let _isRefreshing = false;
let _pendingQueue: PendingEntry[] = [];

/**
 * Registers the function used to refresh the access token on 401.
 * Must be called once at app startup (see UseAuthentication).
 * Injected instead of imported to avoid a circular dependency with AuthenticationService.
 */
export function registerRefreshFn(fn: RefreshFn): void {
  _refreshFn = fn;
}

function _processQueue(error: unknown): void {
  const queue = _pendingQueue.splice(0);
  queue.forEach(({ retryFn, resolve, reject }) => {
    if (error) {
      reject(error);
    } else {
      retryFn().then(resolve).catch(reject);
    }
  });
}

// Arrow wrapper avoids "Illegal invocation" on Hermes — native fetch polyfills cannot be bound.
const _nativeFetch = (input: RequestInfo | URL, init?: RequestInit) => fetch(input, init);

async function _doRefresh(): Promise<void> {
  if (!_refreshFn) {
    throw new Error('No refresh function registered. Call registerRefreshFn() at startup.');
  }
  const newToken = await _refreshFn();
  TokenStorageService.setToken(newToken);
  TokenStorageService.triggerTokenRefreshed(newToken);
}

async function _handle401(retryFn: () => Promise<Response>): Promise<Response> {
  if (_isRefreshing) {
    return new Promise<Response>((resolve, reject) => {
      _pendingQueue.push({ retryFn, resolve, reject });
    });
  }

  _isRefreshing = true;
  try {
    await _doRefresh();
    _processQueue(null);
    return retryFn();
  } catch (err) {
    _processQueue(err);
    TokenStorageService.clearToken();
    TokenStorageService.triggerLogout();
    Toast.show({ type: 'info', text1: 'Sua sessão expirou. Faça login novamente.' });
    throw new SessionExpiredError();
  } finally {
    _isRefreshing = false;
  }
}

async function _internalFetch(
  input: RequestInfo | URL,
  init: RequestInit | undefined,
  isRetry: boolean,
): Promise<Response> {
  const token = TokenStorageService.getToken();
  const initHeaders = (init?.headers as Record<string, string>) ?? {};
  const headers: Record<string, string> = {
    ...initHeaders,
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  };

  const response = await _nativeFetch(input, {
    credentials: 'include',
    ...init,
    headers,
  });

  if (response.status === 401 && !isRetry) {
    return _handle401(() => _internalFetch(input, init, true));
  }

  return response;
}

/**
 * Authorized fetch: drops in as a replacement for the native `fetch`.
 * - Automatically injects `Authorization: Bearer <token>` header.
 * - On 401, delegates to the registered refresh function, then retries the original request.
 * - Multiple concurrent 401s share a single refresh call (queue-based).
 * - If refresh fails, clears tokens and triggers logout.
 */
export async function authorizedFetch(
  input: RequestInfo | URL,
  init?: RequestInit,
): Promise<Response> {
  return _internalFetch(input, init, false);
}
