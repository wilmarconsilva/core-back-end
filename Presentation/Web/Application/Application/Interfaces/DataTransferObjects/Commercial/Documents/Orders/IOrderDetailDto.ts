import { ICustomerReferenceDto } from '../../../Persons/Customers/ICustomerReferenceDto';
import { IPaymentSaveDto } from '../Payments/IPaymentSaveDto';
import { IOrderProductDetailDto } from './IOrderProductDetailDto';

export interface IOrderDetailDto {
  id: number;
  userId: number;
  customer: ICustomerReferenceDto;
  budgetId?: number;
  payment: IPaymentSaveDto;
  observations?: string;
  customerSignatureImage?: string;
  products: IOrderProductDetailDto[];
}
