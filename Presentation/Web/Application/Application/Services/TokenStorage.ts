const TOKEN_KEY = 'core_access_token';

let _accessToken: string | null = null;
let _logoutCallback: (() => void) | null = null;
let _isLoggingOut = false;

const _loadFromStorage = (): void => {
  try {
    if (typeof localStorage !== 'undefined') {
      _accessToken = localStorage.getItem(TOKEN_KEY);
    }
  } catch {
    _accessToken = null;
  }
};

_loadFromStorage();

export const TokenStorage = {
  setToken: (token: string | null): void => {
    _accessToken = token;
    _isLoggingOut = false;
    try {
      if (typeof localStorage !== 'undefined') {
        if (token) {
          localStorage.setItem(TOKEN_KEY, token);
        } else {
          localStorage.removeItem(TOKEN_KEY);
        }
      }
    } catch { /* storage unavailable */ }
  },

  getToken: (): string | null => _accessToken,

  clearToken: (): void => {
    TokenStorage.setToken(null);
  },

  registerLogoutCallback: (callback: () => void): void => {
    _logoutCallback = callback;
  },

  triggerLogout: (): void => {
    _isLoggingOut = true;
    _logoutCallback?.();
  },

  isLoggingOut: (): boolean => _isLoggingOut,
};
