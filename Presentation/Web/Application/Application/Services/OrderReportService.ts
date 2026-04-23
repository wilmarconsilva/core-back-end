import { APIConsumerService } from './APIConsumerService';
import { IOrderReportService } from '../Interfaces/Services/IOrderReportService';
import { OrderReportFilter } from '../Interfaces/DataTransferObjects/OrderReportFilter';
import { OrderReportListing } from '../Interfaces/DataTransferObjects/OrderReportListing';
import { TokenStorage } from './TokenStorage';

function buildQueryParams(obj: unknown): string {
  if (!obj || typeof obj !== 'object') return '';
  return Object.entries(obj as Record<string, unknown>)
    .filter(([, v]) => v !== undefined && v !== null && v !== '')
    .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(String(v))}`)
    .join('&');
}

export class OrderReportService extends APIConsumerService implements IOrderReportService {
  async getReport(filter: OrderReportFilter): Promise<OrderReportListing> {
    const token = TokenStorage.getToken();
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    };
    const params = buildQueryParams(filter);
    return this.request<OrderReportListing>(`/ordersreport?${params}`, {
      method: 'GET',
      credentials: 'include',
      headers,
    });
  }
}
