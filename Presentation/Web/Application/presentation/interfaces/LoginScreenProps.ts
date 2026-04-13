import { LoginRequest, LoginValidationErrors } from '../../application/interfaces/DataTransferObjects/authentication.types';

export interface LoginScreenProps {
  isLoading: boolean;
  error: string | null;
  validationErrors: LoginValidationErrors;
  onLogin: (request: LoginRequest) => void;
  onClearError: () => void;
}