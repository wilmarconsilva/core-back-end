import { DocumentReportListing } from './DocumentReportListing';
import { OrderReport } from './OrderReport';
import { OrderReportSummary } from './OrderReportSummary';

export interface OrderReportListing extends DocumentReportListing<OrderReport, OrderReportSummary> {}
