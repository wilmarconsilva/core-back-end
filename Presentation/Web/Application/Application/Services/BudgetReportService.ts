import { APIConsumerService } from './APIConsumerService';
import { IBudgetReportService } from '../Interfaces/Services/IBudgetReportService';
import { BudgetReportFilter } from '../Interfaces/DataTransferObjects/BudgetReportFilter';
import { BudgetReportListing } from '../Interfaces/DataTransferObjects/BudgetReportListing';
import { TokenStorage } from './TokenStorage';

function buildQueryParams(obj: unknown): string {
  if (!obj || typeof obj !== 'object') return '';
  return Object.entries(obj as Record<string, unknown>)
    .filter(([, v]) => v !== undefined && v !== null && v !== '')
    .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(String(v))}`)
    .join('&');
}

export class BudgetReportService extends APIConsumerService implements IBudgetReportService {
  async getReport(filter: BudgetReportFilter): Promise<BudgetReportListing> {
    const token = TokenStorage.getToken();
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    };
    const params = buildQueryParams(filter);
    return this.request<BudgetReportListing>(`/budgetsreport?${params}`, {
      method: 'GET',
      credentials: 'include',
      headers,
    });
  }
}
