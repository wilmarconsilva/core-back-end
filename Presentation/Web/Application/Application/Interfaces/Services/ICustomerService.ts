import { ICrudService } from './ICrudService';
import { CustomerSaveDto } from '../DataTransferObjects/CustomerSaveDto';
import { CustomerListing } from '../DataTransferObjects/CustomerListing';
import { CustomerListingFilter } from '../DataTransferObjects/CustomerListingFilter';

export interface ICustomerService extends ICrudService<CustomerSaveDto, CustomerListing, CustomerListingFilter> {}
