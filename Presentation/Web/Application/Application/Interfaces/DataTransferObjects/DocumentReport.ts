import { PaymentMethod } from './PaymentMethod';

export interface DocumentReport {
  id: number;
  creationDateTime: string;
  lastUpdateDateTime: string;
  valueSubtotal: number;
  valueTotal: number;
  paymentMethod: PaymentMethod;
  paymentMethodDescription: string;
  paymentDiscountValue: number;
  paymentInstallmentQuantity?: number;
  paymentInstallmentValue?: number;
  paymentInstallmentDownPaymentValue?: number;
  paymentInstallmentDownPaymentPercentage?: number;
  paymentInstallmentDownPaymentMethod?: PaymentMethod;
  paymentInstallmentDownPaymentMethodDescription?: string;
  observations?: string;
  userName: string;
  customerName: string;
  customerCityState: string;
}
