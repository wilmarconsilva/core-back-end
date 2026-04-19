import Toast from 'react-native-toast-message';
import { ApiError } from '../Interfaces/DataTransferObjects/ApiError';

if (!process.env?.EXPO_PUBLIC_API_BASE_URL?.trim()) {
  throw new Error('EXPO_PUBLIC_API_BASE_URL is not defined in the .env file.');
}

const API_BASE_URL = process.env.EXPO_PUBLIC_API_BASE_URL as string;

export interface RequestOptions<T> {
  onSuccess?: (data: T) => void;
  onError?: (error: ApiError) => void;
  successMessage?: string;
  errorMessage?: string;
}

export class APIConsumerService {

  protected async request<T>(
    path: string,
    requestInit: RequestInit,
    options?: RequestOptions<T>,
  ): Promise<T> {
    try {
      const response = await fetch(`${API_BASE_URL}${path}`, requestInit);

      if (!response.ok) {
        const error: ApiError = await response.json().finally(() => ({ status: response.status }));
        throw error;
      }

      const data: T = response.status === 204
        ? undefined as T
        : await (response.json() as Promise<T>);

      if (options?.successMessage) {
        Toast.show({ type: 'success', text1: options.successMessage });
      }

      options?.onSuccess?.(data);

      return data;
    } catch (err: unknown) {
      const apiError = err as ApiError;
      const message = options?.errorMessage
        ?? apiError.detail
        ?? apiError.title
        ?? 'Ocorreu um erro inesperado.';

      Toast.show({ type: 'error', text1: message });

      options?.onError?.(apiError);

      throw apiError;
    }
  }

}