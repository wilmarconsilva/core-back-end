import { CrudService } from './CrudService';
import { ICustomerService } from '../Interfaces/Services/ICustomerService';
import { CustomerSaveDto } from '../Interfaces/DataTransferObjects/CustomerSaveDto';
import { CustomerListing } from '../Interfaces/DataTransferObjects/CustomerListing';
import { CustomerListingFilter } from '../Interfaces/DataTransferObjects/CustomerListingFilter';

export class CustomerService
  extends CrudService<CustomerSaveDto, CustomerListing, CustomerListingFilter>
  implements ICustomerService
{
  constructor() {
    super('/customers');
  }
}
