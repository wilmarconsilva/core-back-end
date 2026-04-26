import { IBudgetReportFilter } from '../DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportFilter';
import { IBudgetReportListing } from '../DataTransferObjects/Commercial/Documents/Budgets/IBudgetReportListing';

export interface IBudgetReportService {
  getReport(filter: IBudgetReportFilter): Promise<IBudgetReportListing>;
}