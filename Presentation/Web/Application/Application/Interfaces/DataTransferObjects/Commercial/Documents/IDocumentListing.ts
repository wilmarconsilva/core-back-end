import { IEntityListing } from '../../IEntityListing';

export interface IDocumentListing extends IEntityListing {
  custormerName: string;
  customerLocalityState: string;
  valueTotal: number;
}
