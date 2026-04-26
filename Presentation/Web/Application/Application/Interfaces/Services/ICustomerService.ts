import { ICrudService } from './ICrudService';
import { ICustomerSaveDto } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { ICustomerListing } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../IListingFilter';

export interface ICustomerService extends ICrudService<ICustomerSaveDto, IICustomerListing, ICustomerListingFilter> {}
