import { PaymentMethod } from './PaymentMethod';
import { InstallmentSaveDto } from './InstallmentSaveDto';

export interface PaymentSaveDto {
  method: PaymentMethod;
  discountValue: number;
  installment?: InstallmentSaveDto;
}
