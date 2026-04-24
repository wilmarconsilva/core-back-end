import { TokenStorage } from './TokenStorage';

if (!process.env?.EXPO_PUBLIC_API_BASE_URL?.trim()) {
  throw new Error('EXPO_PUBLIC_API_BASE_URL is not defined in the .env file.');
}

export const API_BASE_URL = process.env.EXPO_PUBLIC_API_BASE_URL as string;

/** Thrown when a 401 triggers logout (to suppress redundant error toasts). */
export class SessionExpiredError extends Error {
  constructor() {
    super('Session expired');
    this.name = 'SessionExpiredError';
  }
}

type PendingEntry = {
  retryFn: () => Promise<Response>;
  resolve: (value: Response) => void;
  reject: (reason: unknown) => void;
};

let _isRefreshing = false;
let _pendingQueue: PendingEntry[] = [];

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

async function _refreshToken(): Promise<string> {
  const response = await _nativeFetch(`${API_BASE_URL}/authentication/refresh`, {
    method: 'POST',
    credentials: 'include',
  });
  if (!response.ok) {
    throw new Error('Token refresh failed');
  }
  const data = await response.json() as { accessToken: string };
  return data.accessToken;
}

function _handle401(retryFn: () => Promise<Response>): Promise<Response> {
  if (_isRefreshing) {
    return new Promise<Response>((resolve, reject) => {
      _pendingQueue.push({ retryFn, resolve, reject });
    });
  }

  _isRefreshing = true;

  return _refreshToken()
    .then((newToken) => {
      TokenStorage.setToken(newToken);
      _processQueue(null);
      return retryFn();
    })
    .catch((err) => {
      _processQueue(err);
      TokenStorage.clearToken();
      TokenStorage.triggerLogout();
      return Promise.reject(new SessionExpiredError());
    })
    .finally(() => {
      _isRefreshing = false;
    });
}

async function _internalFetch(
  input: RequestInfo | URL,
  init: RequestInit | undefined,
  isRetry: boolean,
): Promise<Response> {
  const token = TokenStorage.getToken();
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
 * - On 401, attempts a single token refresh then retries the original request.
 * - Multiple concurrent 401s share a single refresh call (queue-based).
 * - If refresh fails, clears tokens and triggers logout.
 */
export async function authorizedFetch(
  input: RequestInfo | URL,
  init?: RequestInit,
): Promise<Response> {
  return _internalFetch(input, init, false);
}
