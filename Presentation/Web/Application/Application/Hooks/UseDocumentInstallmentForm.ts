import { useEffect, useState } from 'react';
import { PaymentMethod } from '../Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IPaymentSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Payments/IPaymentSaveDto';
import {
  DocumentInstallmentFormState,
  InstallmentFormService,
} from '../Services/Commercial/Documents/Payments/InstallmentFormService';
import { formatCurrencyInput } from '../Services/Utilities/CurrencyInputUtilities';

export function UseDocumentInstallmentForm(valueTotal: number) {
  const [state, setState] = useState<DocumentInstallmentFormState>(
    InstallmentFormService.createInitialState(),
  );

  useEffect(() => {
    setState((previous) => InstallmentFormService.recalculateOnTotalChange(previous, valueTotal));
  }, [valueTotal]);

  const installmentValue = formatCurrencyInput(
    InstallmentFormService.calculateInstallmentValue(
      valueTotal,
      state.installmentQty,
      state.downPaymentValue,
      state.hasDownPayment,
    ),
  );

  const loadFromPayment = (payment: IPaymentSaveDto) => {
    setState(InstallmentFormService.fromPayment(payment));
  };

  const setHasInstallment = (hasInstallment: boolean) => {
    setState((previous) => InstallmentFormService.applyHasInstallment(previous, hasInstallment));
  };

  const setInstallmentQty = (installmentQty: number) => {
    setState((previous) => ({ ...previous, installmentQty }));
  };

  const setHasDownPayment = (hasDownPayment: boolean) => {
    setState((previous) => InstallmentFormService.applyHasDownPayment(previous, hasDownPayment));
  };

  const setDownPaymentMethod = (downPaymentMethod: PaymentMethod) => {
    setState((previous) => ({ ...previous, downPaymentMethod }));
  };

  const setDownPaymentValue = (downPaymentValue: string) => {
    setState((previous) =>
      InstallmentFormService.syncFromDownPaymentValue(previous, downPaymentValue, valueTotal),
    );
  };

  const setDownPaymentPercentage = (downPaymentPercentage: number) => {
    setState((previous) =>
      InstallmentFormService.syncFromDownPaymentPercentage(
        previous,
        downPaymentPercentage,
        valueTotal,
      ),
    );
  };

  return {
    hasInstallment: state.hasInstallment,
    setHasInstallment,
    installmentQty: state.installmentQty,
    setInstallmentQty,
    installmentValue,
    hasDownPayment: state.hasDownPayment,
    setHasDownPayment,
    downPaymentMethod: state.downPaymentMethod,
    setDownPaymentMethod,
    downPaymentValue: state.downPaymentValue,
    setDownPaymentValue,
    downPaymentPercentage: state.downPaymentPercentage,
    setDownPaymentPercentage,
    loadFromPayment,
    getInstallmentState: () => state,
  };
}
