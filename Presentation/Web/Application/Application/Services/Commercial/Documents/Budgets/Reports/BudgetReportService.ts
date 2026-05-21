import { IBudgetReportService } from '../../../../../Interfaces/Services/IBudgetReportService';
import { IBudgetReportFilter } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportFilter';
import { IBudgetReportListing } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportListing';
import { ReportService } from '../../../../Reports/ReportService';

export class BudgetReportService extends ReportService implements IBudgetReportService {
  async getReport(filter: IBudgetReportFilter): Promise<IBudgetReportListing> {
    return super.getReport(filter, '/budgetsreport');
  }
}
