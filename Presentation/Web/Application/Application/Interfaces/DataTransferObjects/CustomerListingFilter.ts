import { EntityListingFilter } from './EntityListingFilter';

export interface CustomerListingFilter extends EntityListingFilter {
  name?: string;
}
