import { DBSchema } from 'idb';

export const INDEXED_DB_NAME = 'core-offline-db';
export const INDEXED_DB_VERSION = 4;

export const OfflineStoreNames = {
  products: 'products',
  customers: 'customers',
  orders: 'orders',
  budgets: 'budgets',
  localities: 'localities',
} as const;

export type OfflineStoreName =
  | typeof OfflineStoreNames.products
  | typeof OfflineStoreNames.customers
  | typeof OfflineStoreNames.orders
  | typeof OfflineStoreNames.budgets
  | typeof OfflineStoreNames.localities;

export interface CoreDbSchema extends DBSchema {
  products: { key: number; value: { id: number } & Record<string, unknown> };
  customers: { key: number; value: { id: number } & Record<string, unknown> };
  orders: { key: number; value: { id: number } & Record<string, unknown> };
  budgets: { key: number; value: { id: number } & Record<string, unknown> };
  localities: { key: number; value: { id: number } & Record<string, unknown> };
}
