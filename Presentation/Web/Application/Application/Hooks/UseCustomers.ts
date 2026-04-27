import { useMemo } from 'react';
import { CustomerService } from '../Services/CustomerService';
import { ICustomerSaveDto } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { ICustomerListing } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const customerService = new CustomerService();

export function UseCustomers(): UseCrudResult<ICustomerSaveDto, ICustomerListing, ICustomerListingFilter> {
  const service = useMemo(() => customerService, []);
  return UseCrud(service);
}
