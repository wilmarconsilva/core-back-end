import { DownPaymentSaveDto } from './DownPaymentSaveDto';

export interface InstallmentSaveDto {
  quantity: number;
  downPayment?: DownPaymentSaveDto;
}
