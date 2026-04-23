import { OrderReportFilter } from '../DataTransferObjects/OrderReportFilter';
import { OrderReportListing } from '../DataTransferObjects/OrderReportListing';

export interface IOrderReportService {
  getReport(filter: OrderReportFilter): Promise<OrderReportListing>;
}
