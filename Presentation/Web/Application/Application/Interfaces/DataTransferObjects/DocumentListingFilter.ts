import { EntityListingFilter } from './EntityListingFilter';

export interface DocumentListingFilter extends EntityListingFilter {
  creationDateTimeStart?: string;
  creationDateTimeEnd?: string;
  customerId?: number;
  localityId?: number;
}
