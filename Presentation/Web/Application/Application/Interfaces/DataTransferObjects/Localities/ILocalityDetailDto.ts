import { IStateReferenceDto } from '../Territories/IStateReferenceDto';

export interface ILocalityDetailDto {
  id: number;
  description: string;
  state?: IStateReferenceDto;
}
