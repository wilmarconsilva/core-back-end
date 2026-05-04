import { IProductReferenceDto } from '../../../Products/IProductReferenceDto';

export interface IOrderProductDetailDto {
  id: number;
  product: IProductReferenceDto;
  quantity: number;
  valueUnit: number;
  valueTotal: number;
}
