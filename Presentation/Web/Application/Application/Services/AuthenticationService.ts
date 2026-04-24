import { IAuthenticationService } from '../Interfaces/Services/IAuthenticationService';
import { AccessToken } from '../Interfaces/DataTransferObjects/AccessToken';
import { LoginRequest } from '../Interfaces/DataTransferObjects/LoginRequest';
import { APIConsumerService } from './APIConsumerService';

export class AuthenticationService extends APIConsumerService implements IAuthenticationService {

  constructor() {
    // Use plain fetch so auth endpoints are never intercepted by the 401 handler,
    // preventing circular refresh loops.
    super(fetch);
  }

  async login(request: LoginRequest): Promise<AccessToken> {
    return this.request<AccessToken>('/authentication/login', {
      method: 'POST',
      credentials: 'include',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request),
    });
  }

  async refresh(): Promise<AccessToken> {
    return this.request<AccessToken>('/authentication/refresh', { method: 'POST', credentials: 'include' }, { ignoreErrors: true });
  }

  async revoke(): Promise<void> {
    await this.request<void>('/authentication/revoke', { method: 'POST', credentials: 'include' });
  }

}
