import { DBSchema } from 'idb';

export const INDEXED_DB_NAME = 'core-offline-db';
export const INDEXED_DB_VERSION = 1;

export const OfflineStoreNames = {
  products: 'products',
  customers: 'customers',
  pendingOrders: 'pendingOrders',
  pendingBudgets: 'pendingBudgets',
} as const;

export type OfflineReadStoreName =
  | typeof OfflineStoreNames.products
  | typeof OfflineStoreNames.customers;

export type OfflinePendingStoreName =
  | typeof OfflineStoreNames.pendingOrders
  | typeof OfflineStoreNames.pendingBudgets;

interface PendingRecord {
  localId?: number;
  payload: unknown;
  createdAt: string;
}

export interface CoreDbSchema extends DBSchema {
  products: { key: number; value: { id: number } & Record<string, unknown> };
  customers: { key: number; value: { id: number } & Record<string, unknown> };
  pendingOrders: { key: number; value: PendingRecord };
  pendingBudgets: { key: number; value: PendingRecord };
}
