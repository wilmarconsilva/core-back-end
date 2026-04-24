import { APIConsumerService } from '../APIConsumerService';
import { IBudgetReportService } from '../../Interfaces/Services/IBudgetReportService';
import { BudgetReportFilter } from '../../Interfaces/DataTransferObjects/BudgetReportFilter';
import { BudgetReportListing } from '../../Interfaces/DataTransferObjects/BudgetReportListing';
import { buildQueryParams } from '../QueryUtils';

export class BudgetReportService extends APIConsumerService implements IBudgetReportService {
  async getReport(filter: BudgetReportFilter): Promise<BudgetReportListing> {
    const params = buildQueryParams(filter);
    return this.request<BudgetReportListing>(`/budgetsreport?${params}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
