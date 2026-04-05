import { AccessToken } from '../../../domain/authentication/entities/AccessToken';
import { LoginCredentials } from '../../../domain/authentication/entities/LoginCredentials';
import { AuthenticationRepositoryFactory } from '../../../domain/authentication/repositories/AuthenticationRepositoryFactory';
import { IAuthenticationService } from '../interfaces/IAuthenticationService';

export class AuthenticationService implements IAuthenticationService {
  public constructor(private readonly authenticationRepositoryFactory: AuthenticationRepositoryFactory) {}

  public async login(apiBaseUrl: string, credentials: LoginCredentials): Promise<AccessToken> {
    return this.authenticationRepositoryFactory.create(apiBaseUrl).login(credentials);
  }

  public async refresh(apiBaseUrl: string): Promise<AccessToken> {
    return this.authenticationRepositoryFactory.create(apiBaseUrl).refresh();
  }

  public async logout(apiBaseUrl: string): Promise<void> {
    await this.authenticationRepositoryFactory.create(apiBaseUrl).logout();
  }
}
