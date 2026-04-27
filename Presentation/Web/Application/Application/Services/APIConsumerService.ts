import Toast from 'react-native-toast-message';
import { IApiError } from '../Interfaces/DataTransferObjects/Authentication/IApiError';
import { API_BASE_URL, authorizedFetch, SessionExpiredError } from './HttpClient';
import { TokenStorageService } from './TokenStorageService';

type FetchFn = (input: RequestInfo | URL, init?: RequestInit) => Promise<Response>;

export class APIConsumerService {

  constructor(private readonly fetchFn: FetchFn = authorizedFetch) {}

  protected async request<T>(
    path: string,
    requestInit: RequestInit,
  ): Promise<T> {
    try {
      const response = await this.fetchFn(`${API_BASE_URL}${path}`, requestInit);

      if (!response.ok) 
      {
        let error: IApiError = {};

        const text = await response.text();
        try {
          error = JSON.parse(text);
        } catch {
          error = { detail: text };
        }

        throw { ...error, status: response.status };
      }

      const data: T = response.status === 204
        ? undefined as T
        : await (response.json() as Promise<T>);

      Toast.show({ type: 'success', text1: 'Operação realizada com sucesso!' });

      return data;
    }
    catch (err: any) {
      if (err instanceof SessionExpiredError) {
        throw err;
      }

      const apiError = err as IApiError;

      if (!TokenStorageService.isLoggingOut()) 
      {        
        const isBadRequest: boolean = apiError.status !== undefined &&
          apiError.status !== null &&
          apiError.status >= 400 &&
          apiError.status < 500;

        const genericErrorMessage = `Status: ${apiError.status ?? "não definido."} - ${new Date().toLocaleString('pt-BR')} - Ocorreu um erro inesperado. Por favor, contate o suporte.`;

        const message = isBadRequest ? apiError.detail ?? genericErrorMessage : genericErrorMessage;
          
        Toast.show({ type: isBadRequest ? 'info' : 'error', text1: message });
      }
      
      throw apiError;
    }
  }

}