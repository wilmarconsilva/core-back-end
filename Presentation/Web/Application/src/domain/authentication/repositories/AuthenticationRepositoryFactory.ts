import { AuthenticationRepository } from './AuthenticationRepository';

export interface AuthenticationRepositoryFactory {
  create(apiBaseUrl: string): AuthenticationRepository;
}
