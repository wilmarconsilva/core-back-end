import { CrudService } from '../CrudService';
import { IOrderService } from '../../Interfaces/Services/IOrderService';
import { IOrderSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IOrderListing } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListing';
import { IOrderListingFilter } from '../IListingFilter';

export class OrderService
  extends CrudService<IOrderSaveDto, IIOrderListing, IOrderListingFilter>
  implements IOrderService
{
  constructor() {
    super('/orders');
  }
}
