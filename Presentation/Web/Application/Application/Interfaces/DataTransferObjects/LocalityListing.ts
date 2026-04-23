import { EntityListing } from './EntityListing';

export interface LocalityListing extends EntityListing {
  description: string;
  stateId?: number;
}
