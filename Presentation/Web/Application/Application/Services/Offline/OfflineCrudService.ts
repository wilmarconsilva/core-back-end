import { ICrudService } from '../../Interfaces/Services/ICrudService';
import { IOfflineReadRepository } from '../../Interfaces/Persistence/IOfflineReadRepository';
import { OperationModeService } from './OperationModeService';
import { blockOfflineOperation } from './OfflineUnavailableError';

/**
 * Decorator over an API-backed CRUD service that, depending on the operation
 * mode resolved at startup, routes reads either to the API (online) or to the
 * local IndexedDB snapshot (offline). Mutations are blocked while offline.
 *
 * The mode is read per call so the decorator can be built before the mode is
 * resolved (services are module-level singletons created at import time).
 */
export class OfflineCrudService<TSaveDto, TDetailDto, TListing, TFilter>
  implements ICrudService<TSaveDto, TDetailDto, TListing, TFilter> {
  constructor(
    private readonly apiService: ICrudService<TSaveDto, TDetailDto, TListing, TFilter>,
    private readonly repository: IOfflineReadRepository<TListing, TFilter>,
  ) {}

  async getAll(filter: TFilter): Promise<TListing[]> {
    if (OperationModeService.isOffline()) {
      return this.repository.getAll(filter);
    }
    return this.apiService.getAll(filter);
  }

  async getById(id: number): Promise<TDetailDto> {
    if (OperationModeService.isOffline()) {
      return blockOfflineOperation();
    }
    return this.apiService.getById(id);
  }

  async create(dto: TSaveDto): Promise<TSaveDto> {
    if (OperationModeService.isOffline()) {
      return blockOfflineOperation();
    }
    return this.apiService.create(dto);
  }

  async update(id: number, dto: TSaveDto): Promise<TSaveDto> {
    if (OperationModeService.isOffline()) {
      return blockOfflineOperation();
    }
    return this.apiService.update(id, dto);
  }

  async delete(id: number): Promise<void> {
    if (OperationModeService.isOffline()) {
      return blockOfflineOperation();
    }
    return this.apiService.delete(id);
  }
}
