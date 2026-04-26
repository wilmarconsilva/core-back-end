import { useMemo } from 'react';
import { OrderService } from '../Services/Orders/OrderService';
import { IOrderSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IOrderListing } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListing';
import { IOrderListingFilter } from '../IListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const orderService = new OrderService();

export function UseOrders(): UseCrudResult<IOrderSaveDto, IIOrderListing, IOrderListingFilter> {
  const service = useMemo(() => orderService, []);
  return UseCrud(service);
}
