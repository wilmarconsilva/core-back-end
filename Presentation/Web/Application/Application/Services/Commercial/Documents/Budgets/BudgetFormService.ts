import { IValidationRule } from '../../../../Hooks/UseFormValidation';
import { PaymentMethod } from '../../../../Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IBudgetDetailDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetDetailDto';
import { IBudgetProductSaveDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetProductSaveDto';
import { IBudgetSaveDto } from '../../../../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetSaveDto';
import { DocumentInstallmentFormState } from '../Payments/InstallmentFormService';
import { formatCurrencyInput } from '../../../Utilities/CurrencyInputUtilities';
import { CommercialDocumentFormService } from '../CommercialDocumentFormService';

export interface BudgetFormState {
  customerId: number;
  customerDisplayLabel: string;
  observations: string;
  signature: string | null;
  paymentMethod: PaymentMethod;
  discountValue: string;
  productLines: IBudgetProductSaveDto[];
}

export class BudgetFormService {
  static createInitialForm(): BudgetFormState {
    return {
      customerId: 0,
      customerDisplayLabel: '',
      observations: '',
      signature: null,
      paymentMethod: PaymentMethod.Check,
      discountValue: '',
      productLines: [CommercialDocumentFormService.createEmptyProductLine<IBudgetProductSaveDto>()],
    };
  }

  static detailToForm(detail: IBudgetDetailDto): BudgetFormState {
    return {
      customerId: detail.customer.id,
      customerDisplayLabel: CommercialDocumentFormService.entitySelectLabel(detail.customer),
      observations: detail.observations ?? '',
      signature: detail.customerSignatureImage ?? null,
      paymentMethod: detail.payment.method,
      discountValue: formatCurrencyInput(detail.payment.discountValue),
      productLines: CommercialDocumentFormService.detailProductsToForm<IBudgetProductSaveDto>(detail.products),
    };
  }

  static buildSaveDto(
    id: number | undefined,
    form: BudgetFormState,
    installment: DocumentInstallmentFormState,
  ): IBudgetSaveDto {
    return {
      id: id ?? 0,
      creationDateTime: new Date(),
      customerId: form.customerId,
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

  static buildValidationRules(form: BudgetFormState): IValidationRule[] {
    return [
      { key: 'customerId', isValid: !!form.customerId },
    ];
  }
}
