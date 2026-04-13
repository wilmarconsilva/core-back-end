import { IAuthenticationService } from '../interfaces/services/IAuthenticationService';
import { AccessToken, ApiError, LoginRequest } from '../interfaces/DataTransferObjects/authentication.types';

const API_BASE_URL = 'http://localhost:5000';

export class AuthenticationService implements IAuthenticationService {
  async login(request: LoginRequest): Promise<AccessToken> {
    return this.request<AccessToken>('/authentication/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(request),
    });
  }

  async refresh(): Promise<AccessToken> {
    return this.request<AccessToken>('/authentication/refresh', { method: 'POST' });
  }

  async revoke(): Promise<void> {
    await this.request<void>('/authentication/revoke', { method: 'POST' });
  }

  private async request<T>(path: string, options: RequestInit): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${path}`, {
      ...options,
      credentials: 'include',
    });

    if (!response.ok) {
      const error: ApiError = await response.json().catch(() => ({ status: response.status }));
      error.status = response.status;
      throw error;
    }

    if (response.status === 204) return undefined as T;
    return response.json() as Promise<T>;
  }
}
