import { BudgetReportFilter } from '../DataTransferObjects/BudgetReportFilter';
import { BudgetReportListing } from '../DataTransferObjects/BudgetReportListing';

export interface IBudgetReportService {
  getReport(filter: BudgetReportFilter): Promise<BudgetReportListing>;
}
