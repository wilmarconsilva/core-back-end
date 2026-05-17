import { IValidationRule } from '../../../../Hooks/UseFormValidation';
import { PaymentMethod } from '../../../../Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IOrderDetailDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderDetailDto';
import { IOrderProductSaveDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderProductSaveDto';
import { IOrderSaveDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { DocumentInstallmentFormState } from '../Payments/InstallmentFormService';
import { formatCurrencyInput } from '../../../Utilities/CurrencyInputUtilities';
import { CommercialDocumentFormService } from '../CommercialDocumentFormService';

export interface OrderFormState {
  customerId: number;
  customerDisplayLabel: string;
  budgetId: string;
  observations: string;
  signature: string | null;
  paymentMethod: PaymentMethod;
  discountValue: string;
  productLines: IOrderProductSaveDto[];
}

export class OrderFormService {
  static createInitialForm(): OrderFormState {
    return {
      customerId: 0,
      customerDisplayLabel: '',
      budgetId: '',
      observations: '',
      signature: null,
      paymentMethod: PaymentMethod.Check,
      discountValue: '',
      productLines: [CommercialDocumentFormService.createEmptyProductLine<IOrderProductSaveDto>()],
    };
  }

  static detailToForm(detail: IOrderDetailDto): OrderFormState {
    return {
      customerId: detail.customer.id,
      customerDisplayLabel: CommercialDocumentFormService.entitySelectLabel(detail.customer),
      budgetId: detail.budgetId ? String(detail.budgetId) : '',
      observations: detail.observations ?? '',
      signature: detail.customerSignatureImage ?? null,
      paymentMethod: detail.payment.method,
      discountValue: formatCurrencyInput(detail.payment.discountValue),
      productLines: CommercialDocumentFormService.detailProductsToForm<IOrderProductSaveDto>(detail.products),
    };
  }

  static buildSaveDto(
    id: number | undefined,
    form: OrderFormState,
    installment: DocumentInstallmentFormState,
  ): IOrderSaveDto {
    return {
      id: id ?? 0,
      creationDateTime: new Date(),
      customerId: form.customerId,
      budgetId: form.budgetId ? parseInt(form.budgetId, 10) : undefined,
      payment: CommercialDocumentFormService.buildPayment(
        form.paymentMethod,
        form.discountValue,
        installment,
      ),
      observations: form.observations.trim() || undefined,
      customerSignatureImage: form.signature ?? undefined,
      products: CommercialDocumentFormService.saveProductLines(form.productLines),
    };
  }

  static buildValidationRules(form: OrderFormState): IValidationRule[] {
    return [
      { key: 'customerId', isValid: !!form.customerId },
    ];
  }
}
