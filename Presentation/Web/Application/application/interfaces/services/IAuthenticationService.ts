import { AccessToken, LoginRequest } from '../DataTransferObjects/authentication.types';

export interface IAuthenticationService {
  login(request: LoginRequest): Promise<AccessToken>;
  refresh(): Promise<AccessToken>;
  revoke(): Promise<void>;
}
