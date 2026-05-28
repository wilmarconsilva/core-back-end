import { useMemo } from 'react';
import { CustomerService } from '../Services/Customers/CustomerService';
import { ICustomerSaveDto } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { ICustomerDetailDto } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerDetailDto';
import { ICustomerListing } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const customerService = new CustomerService();

export function UseCustomers(): UseCrudResult<ICustomerSaveDto, ICustomerDetailDto, ICustomerListing, ICustomerListingFilter> {
  const service = useMemo(() => customerService, []);
  return UseCrud(service);
}
