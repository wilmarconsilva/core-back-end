import { ICustomerReferenceDto } from '../../../Persons/Customers/ICustomerReferenceDto';
import { IPaymentSaveDto } from '../Payments/IPaymentSaveDto';
import { IBudgetProductDetailDto } from './IBudgetProductDetailDto';

export interface IBudgetDetailDto {
  id: number;
  userId: number;
  customer: ICustomerReferenceDto;
  payment: IPaymentSaveDto;
  observations?: string;
  customerSignatureImage?: string;
  products: IBudgetProductDetailDto[];
}
