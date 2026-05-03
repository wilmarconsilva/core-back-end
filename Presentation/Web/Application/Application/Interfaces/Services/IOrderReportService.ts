import { IOrderReportFilter } from '../DataTransferObjects/Commercial/Documents/Orders/IOrderReportFilter';
import { IOrderReportListing } from '../DataTransferObjects/Commercial/Documents/Orders/IOrderReportListing';

export interface IOrderReportService {
  getReport(filter: IOrderReportFilter): Promise<IOrderReportListing>;
}
