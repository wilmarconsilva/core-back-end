import { CrudService } from './CrudService';
import { ILocalityService } from '../Interfaces/Services/ILocalityService';
import { LocalitySaveDto } from '../Interfaces/DataTransferObjects/LocalitySaveDto';
import { LocalityListing } from '../Interfaces/DataTransferObjects/LocalityListing';
import { LocalityListingFilter } from '../Interfaces/DataTransferObjects/LocalityListingFilter';

export class LocalityService
  extends CrudService<LocalitySaveDto, LocalityListing, LocalityListingFilter>
  implements ILocalityService
{
  constructor() {
    super('/localities');
  }
}
