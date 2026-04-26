import { IOrderReportFilter } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderReportFilter';
import { IOrderReportListing } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderReportListing';

export interface IOrderReportService {
  getReport(filter: IOrderReportFilter): Promise<IOrderReportListing>;
}
