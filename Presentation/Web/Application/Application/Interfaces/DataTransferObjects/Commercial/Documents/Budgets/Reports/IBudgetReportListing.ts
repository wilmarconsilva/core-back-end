import { IDocumentReportListing } from '../Reports/IDocumentReportListing';
import { IBudgetReport } from './IBudgetReport';
import { IBudgetReportSummary } from './IBudgetReportSummary';

export interface IBudgetReportListing extends IDocumentReportListing<IBudgetReport, IBudgetReportSummary> {}
