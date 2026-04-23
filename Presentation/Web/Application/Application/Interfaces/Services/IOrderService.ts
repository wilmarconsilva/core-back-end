import { ICrudService } from './ICrudService';
import { OrderSaveDto } from '../DataTransferObjects/OrderSaveDto';
import { OrderListing } from '../DataTransferObjects/OrderListing';
import { OrderListingFilter } from '../DataTransferObjects/OrderListingFilter';

export interface IOrderService extends ICrudService<OrderSaveDto, OrderListing, OrderListingFilter> {}
