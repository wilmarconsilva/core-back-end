import { EntityListingFilter } from './EntityListingFilter';

export interface ProductListingFilter extends EntityListingFilter {
  description?: string;
}
