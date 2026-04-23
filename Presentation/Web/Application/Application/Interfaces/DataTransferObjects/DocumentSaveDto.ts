import { PaymentSaveDto } from './PaymentSaveDto';
import { DocumentProductSaveDto } from './DocumentProductSaveDto';

export interface DocumentSaveDto<TProduct extends DocumentProductSaveDto> {
  id: number;
  userId: number;
  customerId: number;
  budgetId?: number;
  payment: PaymentSaveDto;
  observations?: string;
  customerSignatureImage?: string;
  products: TProduct[];
}
