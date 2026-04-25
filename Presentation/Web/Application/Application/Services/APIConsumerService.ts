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
        const error = await response.json().catch(() => ({})) as ApiError;
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
          ?? 'Ocorreu um erro inesperado.';
        
        const isServerError: boolean = apiError.status !== undefined &&
          apiError.status !== null &&
          apiError.status >= 500 &&
          apiError.status < 600;
          
        Toast.show({ type: isServerError ? 'error' : 'info', text1: message });
      }

      options?.onError?.(apiError);

      throw apiError;
    }
  }

}