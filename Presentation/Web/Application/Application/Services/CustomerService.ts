import { CrudService } from './CrudService';
import { ICustomerService } from '../Interfaces/Services/ICustomerService';
import { ICustomerSaveDto } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { ICustomerListing } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../IListingFilter';

export class CustomerService
  extends CrudService<ICustomerSaveDto, IICustomerListing, ICustomerListingFilter>
  implements ICustomerService
{
  constructor() {
    super('/customers');
  }
}
