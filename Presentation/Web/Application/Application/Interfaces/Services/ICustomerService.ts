import { ICrudService } from './ICrudService';
import { ICustomerSaveDto } from '../DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { ICustomerListing } from '../DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../DataTransferObjects/Persons/Customers/ICustomerListingFilter';

export interface ICustomerService extends ICrudService<ICustomerSaveDto, ICustomerListing, ICustomerListingFilter> {}
