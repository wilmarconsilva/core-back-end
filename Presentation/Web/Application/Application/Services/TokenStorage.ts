let _accessToken: string | null = null;

export const TokenStorage = {
  setToken: (token: string | null) => {
    _accessToken = token;
  },
  getToken: () => _accessToken,
};
