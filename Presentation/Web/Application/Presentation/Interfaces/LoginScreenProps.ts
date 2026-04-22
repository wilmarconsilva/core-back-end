import { LoginRequest } from '../../Application/Interfaces/DataTransferObjects/LoginRequest';
import { LoginValidationErrors } from '../../Application/Interfaces/DataTransferObjects/LoginValidationErrors';

export interface LoginScreenProps {
  isLoading: boolean;
  error: string | null;
  validationErrors: LoginValidationErrors;
  onLogin: (request: LoginRequest) => void;
  onClearError: () => void;
}