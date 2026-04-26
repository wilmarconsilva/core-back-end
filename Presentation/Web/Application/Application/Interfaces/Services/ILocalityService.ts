import { ICrudService } from './ICrudService';
import { ILocalitySaveDto } from '../Interfaces/DataTransferObjects/Localities/ILocalitySaveDto';
import { ILocalityListing } from '../Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../IListingFilter';

export interface ILocalityService extends ICrudService<ILocalitySaveDto, IILocalityListing, ILocalityListingFilter> {}
