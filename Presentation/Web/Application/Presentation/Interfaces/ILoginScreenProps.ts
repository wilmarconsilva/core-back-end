import { ILoginRequest } from '../Interfaces/DataTransferObjects/Authentication/ILoginRequest';
import { ILoginValidationErrors } from '../Interfaces/DataTransferObjects/Authentication/ILoginValidationErrors';

export interface ILoginScreenProps {
  isLoading: boolean;
  error: string | null;
  validationErrors: ILoginValidationErrors;
  onLogin: (request: ILoginRequest) => void;
  onClearError: () => void;
}