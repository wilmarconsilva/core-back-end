import { ITemplateService } from '../Interfaces/Services/ITemplateService';
import { API_BASE_URL, fetchWithAuth } from './HttpClientService';

export class TemplateService implements ITemplateService {
  async getTemplate(name: string): Promise<string> {
    const response = await fetchWithAuth(`${API_BASE_URL}/templates/${name}`, {
      method: 'GET',
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch template: ${response.status}`);
    }

    return response.text();
  }
}
