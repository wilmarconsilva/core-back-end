import { APIConsumerService, RequestOptions } from './APIConsumerService';
import { ICrudService } from '../Interfaces/Services/ICrudService';
import { TokenStorage } from './TokenStorage';

function buildQueryParams(obj: unknown): string {
  if (!obj || typeof obj !== 'object') return '';
  return Object.entries(obj as Record<string, unknown>)
    .filter(([, v]) => v !== undefined && v !== null && v !== '')
    .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(String(v))}`)
    .join('&');
}

export class CrudService<TSaveDto, TListing, TFilter>
  extends APIConsumerService
  implements ICrudService<TSaveDto, TListing, TFilter>
{
  constructor(protected readonly basePath: string) {
    super();
  }

  private getHeaders(): HeadersInit {
    const token = TokenStorage.getToken();
    return {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    };
  }

  protected async authorizedRequest<T>(
    path: string,
    requestInit: RequestInit,
    options?: RequestOptions<T>,
  ): Promise<T> {
    const token = TokenStorage.getToken();
    const headers: HeadersInit = {
      ...(requestInit.headers as Record<string, string> ?? {}),
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    };
    return this.request<T>(path, { ...requestInit, headers }, options);
  }

  async getAll(filter: TFilter): Promise<TListing[]> {
    const params = buildQueryParams(filter);
    return this.authorizedRequest<TListing[]>(`${this.basePath}?${params}`, {
      method: 'GET',
      credentials: 'include',
      headers: this.getHeaders(),
    });
  }

  async getById(id: number): Promise<TSaveDto> {
    return this.authorizedRequest<TSaveDto>(`${this.basePath}/${id}`, {
      method: 'GET',
      credentials: 'include',
      headers: this.getHeaders(),
    });
  }

  async create(dto: TSaveDto): Promise<TSaveDto> {
    return this.authorizedRequest<TSaveDto>(this.basePath, {
      method: 'POST',
      credentials: 'include',
      headers: this.getHeaders(),
      body: JSON.stringify(dto),
    });
  }

  async update(id: number, dto: TSaveDto): Promise<TSaveDto> {
    return this.authorizedRequest<TSaveDto>(`${this.basePath}/${id}`, {
      method: 'PUT',
      credentials: 'include',
      headers: this.getHeaders(),
      body: JSON.stringify(dto),
    });
  }

  async delete(id: number): Promise<void> {
    return this.authorizedRequest<void>(`${this.basePath}/${id}`, {
      method: 'DELETE',
      credentials: 'include',
      headers: this.getHeaders(),
    });
  }
}
