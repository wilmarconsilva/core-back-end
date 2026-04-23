import { ICrudService } from './ICrudService';
import { LocalitySaveDto } from '../DataTransferObjects/LocalitySaveDto';
import { LocalityListing } from '../DataTransferObjects/LocalityListing';
import { LocalityListingFilter } from '../DataTransferObjects/LocalityListingFilter';

export interface ILocalityService extends ICrudService<LocalitySaveDto, LocalityListing, LocalityListingFilter> {}
