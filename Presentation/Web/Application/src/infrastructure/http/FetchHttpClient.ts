import { HttpClient, HttpRequest } from './HttpClient';
import { HttpError } from './HttpError';

export class FetchHttpClient implements HttpClient {
  public constructor(private readonly apiBaseUrl: string) {}

  public async request<TResponse>(request: HttpRequest): Promise<TResponse | null> {
    const response = await fetch(`${this.normalizeBaseUrl()}${request.path}`, {
      method: request.method,
      body: request.body === undefined ? undefined : JSON.stringify(request.body),
      headers: this.createHeaders(request.body),
      credentials: 'include',
    });

    if (response.status === 204) {
      return null;
    }

    const payload = await this.parsePayload(response);
    if (!response.ok) {
      throw new HttpError(this.extractMessage(payload, response.status), response.status);
    }

    return payload as TResponse;
  }

  private normalizeBaseUrl(): string {
    return this.apiBaseUrl.trim().replace(/\/+$/, '');
  }

  private createHeaders(body: unknown): HeadersInit {
    return body === undefined
      ? { Accept: 'application/json' }
      : {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        };
  }

  private async parsePayload(response: Response): Promise<unknown> {
    const contentType = response.headers.get('content-type') ?? '';
    return contentType.includes('application/json') ? response.json() : response.text();
  }

  private extractMessage(payload: unknown, status: number): string {
    if (typeof payload === 'string' && payload.trim()) {
      return payload;
    }

    if (this.isRecord(payload)) {
      const directMessage = this.getString(payload.message) ?? this.getString(payload.title) ?? this.getString(payload.detail);
      if (directMessage) {
        return directMessage;
      }

      const errors = payload.errors;
      if (this.isRecord(errors)) {
        const firstError = Object.values(errors).find(Array.isArray);
        if (Array.isArray(firstError) && firstError.length > 0 && typeof firstError[0] === 'string') {
          return firstError[0];
        }
      }
    }

    return `Não foi possível concluir a requisição (${status}).`;
  }

  private isRecord(value: unknown): value is Record<string, unknown> {
    return typeof value === 'object' && value !== null;
  }

  private getString(value: unknown): string | null {
    return typeof value === 'string' ? value : null;
  }
}
