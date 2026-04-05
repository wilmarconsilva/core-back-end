import { AccessToken } from '../../domain/authentication/entities/AccessToken';
import { LoginCredentials } from '../../domain/authentication/entities/LoginCredentials';
import { AuthenticationRepository } from '../../domain/authentication/repositories/AuthenticationRepository';
import { HttpClient } from '../http/HttpClient';
import { AccessTokenDto } from './dtos/AccessTokenDto';

export class HttpAuthenticationRepository implements AuthenticationRepository {
  public constructor(private readonly httpClient: HttpClient) {}

  public async login(credentials: LoginCredentials): Promise<AccessToken> {
    const response = await this.httpClient.request<AccessTokenDto>({
      method: 'POST',
      path: '/authentication/login',
      body: credentials,
    });

    return this.mapAccessToken(response);
  }

  public async refresh(): Promise<AccessToken> {
    const response = await this.httpClient.request<AccessTokenDto>({
      method: 'POST',
      path: '/authentication/refresh',
    });

    return this.mapAccessToken(response);
  }

  public async logout(): Promise<void> {
    await this.httpClient.request({
      method: 'POST',
      path: '/authentication/revoke',
    });
  }

  private mapAccessToken(response: AccessTokenDto | null): AccessToken {
    if (response === null) {
      throw new Error('A API não retornou o access token.');
    }

    return {
      token: response.accessToken,
      expirationDateTime: response.accessTokenExpirationDateTime,
    };
  }
}
