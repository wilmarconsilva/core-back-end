import { APIConsumerService } from '../APIConsumerService';
import { IBudgetReportService } from '../../Interfaces/Services/IBudgetReportService';
import { IBudgetReportFilter } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportFilter';
import { IBudgetReportListing } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportListing';
import { buildQueryParams } from '../Utilities/QueryUtilities';

export class BudgetReportService extends APIConsumerService implements IBudgetReportService {
  async getReport(filter: IBudgetReportFilter): Promise<IBudgetReportListing> {
    const params = buildQueryParams(filter);
    return this.request<IBudgetReportListing>(`/budgetsreport?${params}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
