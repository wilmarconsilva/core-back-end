import { APIConsumerService } from './APIConsumerService';
import { ITemplateService } from '../Interfaces/Services/ITemplateService';
import { TokenStorage } from './TokenStorage';

const API_BASE_URL = process.env.EXPO_PUBLIC_API_BASE_URL as string;

export class TemplateService extends APIConsumerService implements ITemplateService {
  async getTemplate(name: string): Promise<string> {
    const token = TokenStorage.getToken();
    const headers: HeadersInit = {
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    };

    const response = await fetch(`${API_BASE_URL}/templates/${name}`, {
      method: 'GET',
      credentials: 'include',
      headers,
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch template: ${response.status}`);
    }

    return response.text();
  }
}
