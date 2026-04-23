import { PersonType } from './PersonType';
import { CustomerAddressSaveDto } from './CustomerAddressSaveDto';

export interface CustomerSaveDto {
  id: number;
  name: string;
  personType: PersonType;
  document: string;
  addresses: CustomerAddressSaveDto[];
  email?: string;
  stateRegistration?: string;
}
