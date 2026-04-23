import { DocumentReportListing } from './DocumentReportListing';
import { BudgetReport } from './BudgetReport';
import { BudgetReportSummary } from './BudgetReportSummary';

export interface BudgetReportListing extends DocumentReportListing<BudgetReport, BudgetReportSummary> {}
