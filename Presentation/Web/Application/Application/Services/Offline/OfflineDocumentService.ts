import Toast from 'react-native-toast-message';
import { ICrudService } from '../../Interfaces/Services/ICrudService';
import { IPendingDocumentRepository } from '../../Interfaces/Persistence/IPendingDocumentRepository';
import { OperationModeService } from './OperationModeService';
import { blockOfflineOperation } from './OfflineUnavailableError';

const OFFLINE_CREATED_MESSAGE = 'Registro salvo localmente. Será sincronizado quando houver conexão.';

/**
 * Decorator for document services (Orders / Budgets). Online it delegates to
 * the API service. Offline it only allows creation, persisting the document in
 * the pending queue for later synchronization; all other operations are blocked.
 */
export class OfflineDocumentService<TSaveDto, TDetailDto, TListing, TFilter>
  implements ICrudService<TSaveDto, TDetailDto, TListing, TFilter> {
  constructor(
    private readonly apiService: ICrudService<TSaveDto, TDetailDto, TListing, TFilter>,
    private readonly pendingRepository: IPendingDocumentRepository<TSaveDto>,
  ) {}

  async getAll(filter: TFilter): Promise<TListing[]> {
    if (OperationModeService.isOffline()) {
      return blockOfflineOperation();
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
      await this.pendingRepository.add(dto);
      Toast.show({ type: 'success', text1: OFFLINE_CREATED_MESSAGE });
      return dto;
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
