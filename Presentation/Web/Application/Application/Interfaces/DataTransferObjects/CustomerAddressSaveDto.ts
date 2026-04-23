export interface CustomerAddressSaveDto {
  id: number;
  neighborhood: string;
  street: string;
  localityId: number;
  complement?: string;
  postalCode?: string;
  number?: string;
}
