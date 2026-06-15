import { DBSchema } from 'idb';

export const INDEXED_DB_NAME = 'core-offline-db';
export const INDEXED_DB_VERSION = 3;

export const OfflineStoreNames = {
  products: 'products',
  customers: 'customers',
  orders: 'orders',
  budgets: 'budgets',
} as const;

export type OfflineStoreName =
  | typeof OfflineStoreNames.products
  | typeof OfflineStoreNames.customers
  | typeof OfflineStoreNames.orders
  | typeof OfflineStoreNames.budgets;

export interface CoreDbSchema extends DBSchema {
  products: { key: number; value: { id: number } & Record<string, unknown> };
  customers: { key: number; value: { id: number } & Record<string, unknown> };
  orders: { key: number; value: { id: number } & Record<string, unknown> };
  budgets: { key: number; value: { id: number } & Record<string, unknown> };
}
