import { CrudService } from './CrudService';
import { ILocalityService } from '../Interfaces/Services/ILocalityService';
import { ILocalitySaveDto } from '../Interfaces/DataTransferObjects/Localities/ILocalitySaveDto';
import { ILocalityListing } from '../Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';

export class LocalityService
  extends CrudService<ILocalitySaveDto, ILocalityListing, ILocalityListingFilter>
  implements ILocalityService
{
  constructor() {
    super('/localities');
  }
}
