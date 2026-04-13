import { ApiError } from '../Interfaces/DataTransferObjects/ApiError';

if (!process.env?.API_BASE_URL?.trim()) {
  throw new Error('API_BASE_URL is not defined in the .env file.');
}

const API_BASE_URL = process.env.API_BASE_URL as string;

export class APIConsumerService {

  protected async request<T>(path: string, options: RequestInit): Promise<T> {
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