import { IEntityListing } from '../../IEntityListing';

export interface IDocumentListing extends IEntityListing {
  custormerName: string;
  customerCityState: string;
  valueTotal: number;
}
