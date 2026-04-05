import { IAuthenticationService } from '../application/authentication/interfaces/IAuthenticationService';
import { AuthenticationService } from '../application/authentication/services/AuthenticationService';
import { HttpAuthenticationRepositoryFactory } from '../infrastructure/authentication/HttpAuthenticationRepositoryFactory';

export type AuthenticationDependencies = {
  defaultApiBaseUrl: string;
  authenticationService: IAuthenticationService;
};

export function createAuthenticationDependencies(): AuthenticationDependencies {
  const authenticationRepositoryFactory = new HttpAuthenticationRepositoryFactory();

  return {
    defaultApiBaseUrl: 'https://localhost:7294',
    authenticationService: new AuthenticationService(authenticationRepositoryFactory),
  };
}
