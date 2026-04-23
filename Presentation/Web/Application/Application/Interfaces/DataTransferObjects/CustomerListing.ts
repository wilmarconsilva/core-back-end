import { EntityListing } from './EntityListing';

export interface CustomerListing extends EntityListing {
  name: string;
  document: string;
}
