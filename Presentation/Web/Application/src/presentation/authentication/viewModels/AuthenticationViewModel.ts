export type AuthenticationViewModel = {
  apiBaseUrl: string;
  email: string;
  password: string;
  feedback: string;
  isAuthenticated: boolean;
  isSubmitting: boolean;
  isRestoringSession: boolean;
  accessTokenPreview: string;
  formattedExpiration: string;
  setApiBaseUrl(value: string): void;
  setEmail(value: string): void;
  setPassword(value: string): void;
  login(): Promise<void>;
  refreshSession(): Promise<void>;
  logout(): Promise<void>;
};
