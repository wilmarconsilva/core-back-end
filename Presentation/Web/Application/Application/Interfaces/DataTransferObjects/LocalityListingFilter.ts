import { EntityListingFilter } from './EntityListingFilter';

export interface LocalityListingFilter extends EntityListingFilter {
  description?: string;
  stateId?: number;
}
