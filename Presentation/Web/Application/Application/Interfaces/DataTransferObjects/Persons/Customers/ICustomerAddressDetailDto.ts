import { ILocalityReferenceDto } from '../../Localities/ILocalityReferenceDto';

export interface ICustomerAddressDetailDto {
  id: number;
  neighborhood: string;
  street: string;
  locality: ILocalityReferenceDto;
  complement?: string;
  postalCode?: string;
  number?: string;
}
