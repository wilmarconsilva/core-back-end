import { DocumentReport } from './DocumentReport';
import { DocumentReportSummary } from './DocumentReportSummary';

export interface DocumentReportListing<TReport extends DocumentReport, TSummary extends DocumentReportSummary> {
  registers: TReport[];
  summary?: TSummary;
}
