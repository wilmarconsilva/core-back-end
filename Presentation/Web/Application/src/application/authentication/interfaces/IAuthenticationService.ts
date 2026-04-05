import { AccessToken } from '../../../domain/authentication/entities/AccessToken';
import { LoginCredentials } from '../../../domain/authentication/entities/LoginCredentials';

export interface IAuthenticationService {
  login(apiBaseUrl: string, credentials: LoginCredentials): Promise<AccessToken>;
  refresh(apiBaseUrl: string): Promise<AccessToken>;
  logout(apiBaseUrl: string): Promise<void>;
}
