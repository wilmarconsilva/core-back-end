/**
 * Read-only offline repository used while the application runs in offline mode.
 * Also exposes replaceAll, used by the synchronization process to refresh the
 * local snapshot from the server.
 */
export interface IOfflineReadRepository<TListing, TFilter> {
  getAll(filter: TFilter): Promise<TListing[]>;
  getById(id: number): Promise<TListing | undefined>;
  replaceAll(items: TListing[]): Promise<void>;
}
