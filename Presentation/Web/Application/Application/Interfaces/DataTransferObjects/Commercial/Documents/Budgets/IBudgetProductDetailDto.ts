import { IProductReferenceDto } from '../../../Products/IProductReferenceDto';

export interface IBudgetProductDetailDto {
  id: number;
  product: IProductReferenceDto;
  quantity: number;
  valueUnit: number;
  valueTotal: number;
}
