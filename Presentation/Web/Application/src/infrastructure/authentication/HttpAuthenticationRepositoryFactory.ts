import { AuthenticationRepositoryFactory } from '../../domain/authentication/repositories/AuthenticationRepositoryFactory';
import { AuthenticationRepository } from '../../domain/authentication/repositories/AuthenticationRepository';
import { FetchHttpClient } from '../http/FetchHttpClient';
import { HttpAuthenticationRepository } from './HttpAuthenticationRepository';

export class HttpAuthenticationRepositoryFactory implements AuthenticationRepositoryFactory {
  public create(apiBaseUrl: string): AuthenticationRepository {
    return new HttpAuthenticationRepository(new FetchHttpClient(apiBaseUrl));
  }
}
