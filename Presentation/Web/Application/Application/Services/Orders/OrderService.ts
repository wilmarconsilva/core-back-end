import { CrudService } from '../CrudService';
import { IOrderService } from '../../Interfaces/Services/IOrderService';
import { IOrderSaveDto } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IOrderListing } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListing';
import { IOrderListingFilter } from '../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListingFilter';

export class OrderService
  extends CrudService<IOrderSaveDto, IOrderListing, IOrderListingFilter>
  implements IOrderService
{
  constructor() {
    super('/orders');
  }
}
