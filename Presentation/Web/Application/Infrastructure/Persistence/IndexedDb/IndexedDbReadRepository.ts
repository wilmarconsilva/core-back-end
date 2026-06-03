import { IListingFilter } from '../../../Application/Interfaces/DataTransferObjects/IListingFilter';
import { IOfflineReadRepository } from '../../../Application/Interfaces/Persistence/IOfflineReadRepository';
import { IIndexedDbContext } from './IIndexedDbContext';
import { OfflineReadStoreName } from './IndexedDbSchema';

export type OfflineFilterPredicate<TListing, TFilter> = (item: TListing, filter: TFilter) => boolean;

/**
 * Generic IndexedDB-backed read repository.
 * Text filtering and pagination are applied in memory because the offline
 * datasets are small and substring matching is not index friendly.
 */
export class IndexedDbReadRepository<
  TListing extends { id: number },
  TFilter extends IListingFilter,
> implements IOfflineReadRepository<TListing, TFilter> {
  constructor(
    private readonly context: IIndexedDbContext,
    private readonly storeName: OfflineReadStoreName,
    private readonly matches: OfflineFilterPredicate<TListing, TFilter>,
  ) {}

  async getAll(filter: TFilter): Promise<TListing[]> {
    const database = await this.context.getDatabase();
    const all = (await database.getAll(this.storeName)) as unknown as TListing[];

    const filtered = all
      .filter((item) => this.matches(item, filter))
      .sort((first, second) => first.id - second.id);

    const start = filter.registersStart ?? 0;
    const end = filter.registersEnd ?? filtered.length;

    return filtered.slice(start, end);
  }

  async getById(id: number): Promise<TListing | undefined> {
    const database = await this.context.getDatabase();
    return (await database.get(this.storeName, id)) as unknown as TListing | undefined;
  }

  async replaceAll(items: TListing[]): Promise<void> {
    const database = await this.context.getDatabase();
    const transaction = database.transaction(this.storeName, 'readwrite');

    // All requests are queued synchronously so the transaction does not
    // auto-commit between awaits (a well known IndexedDB pitfall).
    await Promise.all([
      transaction.store.clear(),
      ...items.map((item) => transaction.store.put(item as never)),
      transaction.done,
    ]);
  }
}
