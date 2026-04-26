import { APIConsumerService } from '../APIConsumerService';
import { IBudgetReportService } from '../../Interfaces/Services/IBudgetReportService';
import { BudgetReportFilter } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportFilter';
import { BudgetReportListing } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportListing';
import { buildQueryParams } from '../Utilities/QueryUtilities';

export class BudgetReportService extends APIConsumerService implements IBudgetReportService {
  async getReport(filter: BudgetReportFilter): Promise<BudgetReportListing> {
    const params = buildQueryParams(filter);
    return this.request<BudgetReportListing>(`/budgetsreport?${params}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
