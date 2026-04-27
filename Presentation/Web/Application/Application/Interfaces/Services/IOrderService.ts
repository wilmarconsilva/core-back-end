import { ICrudService } from './ICrudService';
import { IOrderSaveDto } from '../DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IOrderListing } from '../DataTransferObjects/Commercial/Documents/Orders/IOrderListing';
import { IOrderListingFilter } from '../DataTransferObjects/Commercial/Documents/Orders/IOrderListingFilter';

export interface IOrderService extends ICrudService<IOrderSaveDto, IOrderListing, IOrderListingFilter> {}
