import { CrudService } from './CrudService';
import { ILocalityService } from '../Interfaces/Services/ILocalityService';
import { ILocalitySaveDto } from '../Interfaces/DataTransferObjects/Localities/ILocalitySaveDto';
import { ILocalityListing } from '../Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../IListingFilter';

export class LocalityService
  extends CrudService<ILocalitySaveDto, IILocalityListing, ILocalityListingFilter>
  implements ILocalityService
{
  constructor() {
    super('/localities');
  }
}
