import { ICrudService } from './ICrudService';
import { ILocalitySaveDto } from '../DataTransferObjects/Localities/ILocalitySaveDto';
import { ILocalityListing } from '../DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../DataTransferObjects/Localities/ILocalityListingFilter';

export interface ILocalityService extends ICrudService<ILocalitySaveDto, ILocalityListing, ILocalityListingFilter> {}
