export type HttpMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';

export type HttpRequest = {
  method: HttpMethod;
  path: string;
  body?: unknown;
};

export interface HttpClient {
  request<TResponse>(request: HttpRequest): Promise<TResponse | null>;
}
