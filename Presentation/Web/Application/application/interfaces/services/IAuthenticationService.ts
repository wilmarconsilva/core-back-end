import { AccessToken } from '../DataTransferObjects/AccessToken';
import { LoginRequest } from '../DataTransferObjects/LoginRequest';

export interface IAuthenticationService {
  login(request: LoginRequest): Promise<AccessToken>;
  refresh(): Promise<AccessToken>;
  revoke(): Promise<void>;
}
