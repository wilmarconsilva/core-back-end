import { ICrudService } from './ICrudService';
import { IOrderSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IOrderListing } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListing';
import { IOrderListingFilter } from '../IListingFilter';

export interface IOrderService extends ICrudService<IOrderSaveDto, IIOrderListing, IOrderListingFilter> {}
