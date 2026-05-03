import { IDocumentReport } from './IDocumentReport';
import { IDocumentReportSummary } from './IDocumentReportSummary';

export interface IDocumentReportListing<TReport extends IDocumentReport, TSummary extends IDocumentReportSummary> {
  registers: TReport[];
  summary?: TSummary;
}