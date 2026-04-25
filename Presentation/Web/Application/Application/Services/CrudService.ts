import { APIConsumerService, RequestOptions } from './APIConsumerService';
import { ICrudService } from '../Interfaces/Services/ICrudService';
import { buildQueryParams } from './Utilities/QueryUtilities';

export class CrudService<TSaveDto, TListing, TFilter>
  extends APIConsumerService
  implements ICrudService<TSaveDto, TListing, TFilter>
{
  constructor(protected readonly basePath: string) {
    super();
  }

  async getAll(filter: TFilter): Promise<TListing[]> {
    const params = buildQueryParams(filter);
    return this.request<TListing[]>(`${this.basePath}?${params}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
  }

  async getById(id: number): Promise<TSaveDto> {
    return this.request<TSaveDto>(`${this.basePath}/${id}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
  }

  async create(dto: TSaveDto, options?: RequestOptions<TSaveDto>): Promise<TSaveDto> {
    return this.request<TSaveDto>(this.basePath, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(dto),
    }, options);
  }

  async update(id: number, dto: TSaveDto): Promise<TSaveDto> {
    return this.request<TSaveDto>(`${this.basePath}/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(dto),
    });
  }

  async delete(id: number): Promise<void> {
    return this.request<void>(`${this.basePath}/${id}`, {
      method: 'DELETE',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
