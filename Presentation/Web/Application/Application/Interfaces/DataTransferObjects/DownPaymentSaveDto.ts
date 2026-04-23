import { PaymentMethod } from './PaymentMethod';

export interface DownPaymentSaveDto {
  value: number;
  percentage: number;
  method: PaymentMethod;
}
