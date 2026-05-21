import { IOrderReportService } from '../../../../Interfaces/Services/IOrderReportService';
import { IOrderReportFilter } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderReportFilter';
import { IOrderReportListing } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderReportListing';
import { ReportService } from '../../../Reports/ReportService';

export class OrderReportService extends ReportService implements IOrderReportService {
  async getReport(filter: IOrderReportFilter): Promise<IOrderReportListing> {
    return super.getReport(filter, '/ordersreport');
  }
}
