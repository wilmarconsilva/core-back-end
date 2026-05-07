import { useMemo } from 'react';
import { OrderService } from '../Services/Orders/OrderService';
import { IOrderSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { IOrderDetailDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderDetailDto';
import { IOrderListing } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListing';
import { IOrderListingFilter } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const orderService = new OrderService();

export function UseOrders(): UseCrudResult<IOrderSaveDto, IOrderDetailDto, IOrderListing, IOrderListingFilter> {
  const service = useMemo(() => orderService, []);
  return UseCrud(service);
}
