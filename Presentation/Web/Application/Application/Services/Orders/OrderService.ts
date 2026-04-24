import { CrudService } from '../CrudService';
import { IOrderService } from '../../Interfaces/Services/IOrderService';
import { OrderSaveDto } from '../../Interfaces/DataTransferObjects/OrderSaveDto';
import { OrderListing } from '../../Interfaces/DataTransferObjects/OrderListing';
import { OrderListingFilter } from '../../Interfaces/DataTransferObjects/OrderListingFilter';

export class OrderService
  extends CrudService<OrderSaveDto, OrderListing, OrderListingFilter>
  implements IOrderService
{
  constructor() {
    super('/orders');
  }
}
