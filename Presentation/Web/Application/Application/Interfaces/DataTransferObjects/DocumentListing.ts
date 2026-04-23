import { EntityListing } from './EntityListing';

export interface DocumentListing extends EntityListing {
  custormerName: string;
  customerCityState: string;
  valueTotal: number;
}
