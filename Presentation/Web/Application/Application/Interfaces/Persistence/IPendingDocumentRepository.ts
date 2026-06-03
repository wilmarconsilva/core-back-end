export interface IPendingDocument<TSaveDto> {
  localId: number;
  payload: TSaveDto;
  createdAt: string;
}

/**
 * Queue of documents (Orders / Budgets) created while offline and awaiting
 * synchronization with the server on the next online startup.
 */
export interface IPendingDocumentRepository<TSaveDto> {
  add(payload: TSaveDto): Promise<void>;
  getAll(): Promise<IPendingDocument<TSaveDto>[]>;
  remove(localId: number): Promise<void>;
}
