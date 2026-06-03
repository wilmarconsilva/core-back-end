import {
  IPendingDocument,
  IPendingDocumentRepository,
} from '../../../Application/Interfaces/Persistence/IPendingDocumentRepository';
import { IIndexedDbContext } from './IIndexedDbContext';
import { OfflinePendingStoreName } from './IndexedDbSchema';

/**
 * Generic IndexedDB-backed queue for documents created offline.
 * Records are removed only after a successful synchronization with the server.
 */
export class IndexedDbPendingRepository<TSaveDto>
  implements IPendingDocumentRepository<TSaveDto> {
  constructor(
    private readonly context: IIndexedDbContext,
    private readonly storeName: OfflinePendingStoreName,
  ) {}

  async add(payload: TSaveDto): Promise<void> {
    const database = await this.context.getDatabase();
    await database.add(this.storeName, {
      payload,
      createdAt: new Date().toISOString(),
    } as never);
  }

  async getAll(): Promise<IPendingDocument<TSaveDto>[]> {
    const database = await this.context.getDatabase();
    const records = await database.getAll(this.storeName);
    return records as unknown as IPendingDocument<TSaveDto>[];
  }

  async remove(localId: number): Promise<void> {
    const database = await this.context.getDatabase();
    await database.delete(this.storeName, localId);
  }
}
