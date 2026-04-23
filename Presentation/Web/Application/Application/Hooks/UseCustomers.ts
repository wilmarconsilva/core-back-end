import { useMemo } from 'react';
import { CustomerService } from '../Services/CustomerService';
import { CustomerSaveDto } from '../Interfaces/DataTransferObjects/CustomerSaveDto';
import { CustomerListing } from '../Interfaces/DataTransferObjects/CustomerListing';
import { CustomerListingFilter } from '../Interfaces/DataTransferObjects/CustomerListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const customerService = new CustomerService();

export function UseCustomers(): UseCrudResult<CustomerSaveDto, CustomerListing, CustomerListingFilter> {
  const service = useMemo(() => customerService, []);
  return UseCrud(service);
}
