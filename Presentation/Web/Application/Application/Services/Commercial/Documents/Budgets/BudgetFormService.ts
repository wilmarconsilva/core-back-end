import { IValidationRule } from '../../../../Hooks/UseFormValidation';
import { PaymentMethod } from '../../../../Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IBudgetDetailDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetDetailDto';
import { IBudgetProductSaveDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetProductSaveDto';
import { IBudgetSaveDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { CommercialDocumentFormService } from '../CommercialDocumentFormService';

export interface BudgetFormState {
  customerId: number;
  customerDisplayLabel: string;
  observations: string;
  signature: string | null;
  paymentMethod: PaymentMethod;
  discountValue: string;
  installmentQty: string;
  productLines: IBudgetProductSaveDto[];
}

export class BudgetFormService {
  static createInitialForm(): BudgetFormState {
    return {
      customerId: 0,
      customerDisplayLabel: '',
      observations: '',
      signature: null,
      paymentMethod: PaymentMethod.Money,
      discountValue: '0',
      installmentQty: '',
      productLines: [CommercialDocumentFormService.createEmptyProductLine<IBudgetProductSaveDto>()],
    };
  }

  static detailToForm(detail: IBudgetDetailDto): BudgetFormState {
    return {
      customerId: detail.customer.id,
      customerDisplayLabel: CommercialDocumentFormService.entitySelectLabel(detail.customer),
      observations: detail.observations ?? '',
      signature: detail.customerSignatureImage ?? null,
      paymentMethod: detail.payment?.method ?? PaymentMethod.Money,
      discountValue: String(detail.payment?.discountValue ?? 0),
      installmentQty: String(detail.payment?.installment?.quantity ?? ''),
      productLines: CommercialDocumentFormService.detailProductsToForm<IBudgetProductSaveDto>(detail.products),
    };
  }

  static buildSaveDto(id: number | undefined, form: BudgetFormState): IBudgetSaveDto {
    return {
      id: id ?? 0,
      customerId: form.customerId,
      payment: CommercialDocumentFormService.buildPayment(
        form.paymentMethod,
        form.discountValue,
        form.installmentQty,
      ),
      observations: form.observations.trim() || undefined,
      customerSignatureImage: form.signature ?? undefined,
      products: CommercialDocumentFormService.saveProductLines(form.productLines),
    };
  }

  static buildValidationRules(form: BudgetFormState): IValidationRule[] {
    return [
      { key: 'customerId', isValid: !!form.customerId },
    ];
  }
}
