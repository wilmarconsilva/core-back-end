import { APIConsumerService } from '../../../APIConsumerService';
import { IOrderReportService } from '../../../../Interfaces/Services/IOrderReportService';
import { IOrderReportFilter } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderReportFilter';
import { IOrderReportListing } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderReportListing';
import { buildQueryParams } from '../../../Utilities/QueryUtilities';

export class OrderReportService extends APIConsumerService implements IOrderReportService {
  async getReport(filter: IOrderReportFilter): Promise<IOrderReportListing> {
    const params = buildQueryParams(filter);
    return this.request<IOrderReportListing>(`/ordersreport?${params}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
