import { useMemo } from 'react';
import { OrderService } from '../Services/Orders/OrderService';
import { OrderSaveDto } from '../Interfaces/DataTransferObjects/OrderSaveDto';
import { OrderListing } from '../Interfaces/DataTransferObjects/OrderListing';
import { OrderListingFilter } from '../Interfaces/DataTransferObjects/OrderListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const orderService = new OrderService();

export function UseOrders(): UseCrudResult<OrderSaveDto, OrderListing, OrderListingFilter> {
  const service = useMemo(() => orderService, []);
  return UseCrud(service);
}
