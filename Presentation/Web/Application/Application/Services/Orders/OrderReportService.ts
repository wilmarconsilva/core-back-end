import { APIConsumerService } from '../APIConsumerService';
import { IOrderReportService } from '../../Interfaces/Services/IOrderReportService';
import { OrderReportFilter } from '../../Interfaces/DataTransferObjects/OrderReportFilter';
import { OrderReportListing } from '../../Interfaces/DataTransferObjects/OrderReportListing';
import { buildQueryParams } from '../QueryUtils';

export class OrderReportService extends APIConsumerService implements IOrderReportService {
  async getReport(filter: OrderReportFilter): Promise<OrderReportListing> {
    const params = buildQueryParams(filter);
    return this.request<OrderReportListing>(`/ordersreport?${params}`, {
      method: 'GET',
      headers: { 'Content-Type': 'application/json' },
    });
  }
}
