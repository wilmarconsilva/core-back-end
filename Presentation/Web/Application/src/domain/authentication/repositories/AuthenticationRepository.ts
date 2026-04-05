import { AccessToken } from '../entities/AccessToken';
import { LoginCredentials } from '../entities/LoginCredentials';

export interface AuthenticationRepository {
  login(credentials: LoginCredentials): Promise<AccessToken>;
  refresh(): Promise<AccessToken>;
  logout(): Promise<void>;
}
