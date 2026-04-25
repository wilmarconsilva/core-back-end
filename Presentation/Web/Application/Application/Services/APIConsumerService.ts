import Toast from 'react-native-toast-message';
import { ApiError } from '../Interfaces/DataTransferObjects/ApiError';
import { API_BASE_URL, authorizedFetch, SessionExpiredError } from './HttpClient';
import { TokenStorage } from './TokenStorage';

export interface RequestOptions<T> {
  onSuccess?: (data: T) => void;
  onError?: (error: ApiError) => void;
  ignoreErrors?: boolean;
}

type FetchFn = (input: RequestInfo | URL, init?: RequestInit) => Promise<Response>;

export class APIConsumerService {

  constructor(private readonly fetchFn: FetchFn = authorizedFetch) {}

  protected async request<T>(
    path: string,
    requestInit: RequestInit,
    options?: RequestOptions<T>,
  ): Promise<T> {
    try {
      const response = await this.fetchFn(`${API_BASE_URL}${path}`, requestInit);

      if (!response.ok) {
        let error: ApiError = {};

        try {
          const text = await response.text();
          try {
            error = JSON.parse(text);
          } catch {
            error = { detail: text };
          }
        } catch {
          // Se tudo falhar, erro vazio
          error = {};
        }

        throw { ...error, status: response.status };
      }

      const data: T = response.status === 204
        ? undefined as T
        : await (response.json() as Promise<T>);

      options?.onSuccess?.(data);

      return data;
    }
    catch (err: unknown) {
      if (err instanceof SessionExpiredError) {
        throw err;
      }

      const apiError = err as ApiError;

      if (!options?.ignoreErrors && !TokenStorage.isLoggingOut()) 
      {
        const errorsMessages = apiError.errors
          ? Object.values(apiError.errors).flat().join('\n')
          : null;
        
        const message = errorsMessages
          ?? apiError.detail
          ?? apiError.title
          ?? `${new Date().toLocaleString('pt-BR')} - Ocorreu um erro inesperado. Por favor, contate o suporte.`;
        
        const isBadRequest: boolean = apiError.status !== undefined &&
          apiError.status !== null &&
          apiError.status >= 400 &&
          apiError.status < 500;
          
        Toast.show({ type: isBadRequest ? 'info' : 'error', text1: message });
      }

      options?.onError?.(apiError);

      throw apiError;
    }
  }

}