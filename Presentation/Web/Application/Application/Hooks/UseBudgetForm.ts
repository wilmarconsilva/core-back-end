import { useEffect, useState } from 'react';
import { UseBudgets } from './UseBudgets';
import { UseCustomers } from './UseCustomers';
import { UseFormValidation } from './UseFormValidation';
import { UseProducts } from './UseProducts';
import { PaymentMethod } from '../Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { ICustomerListing } from '../Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { IBudgetProductSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Budgets/IBudgetProductSaveDto';
import { BudgetFormService } from '../Services/Commercial/Documents/Budgets/BudgetFormService';
import { CommercialDocumentFormService } from '../Services/Commercial/Documents/CommercialDocumentFormService';
import { UseDocumentInstallmentForm } from './UseDocumentInstallmentForm';

const PAGE_SIZE = 50;

export function UseBudgetForm(id: number | undefined, onGoBack: () => void) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseBudgets();
  const { listings: customers, isLoadingList: isLoadingCustomers, loadAll: loadCustomers } = UseCustomers();
  const { listings: products, isLoadingList: isLoadingProducts, loadAll: loadProducts } = UseProducts();
  const { errors, validate, clearError } = UseFormValidation();
  const isEditing = !!id;

  const initialForm = BudgetFormService.createInitialForm();
  const [customerId, setCustomerId] = useState(initialForm.customerId);
  const [customerDisplayLabel, setCustomerDisplayLabel] = useState(initialForm.customerDisplayLabel);
  const [observations, setObservations] = useState(initialForm.observations);
  const [signature, setSignature] = useState<string | null>(initialForm.signature);
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(initialForm.paymentMethod);
  const [discountValue, setDiscountValue] = useState(initialForm.discountValue);
  const [productLines, setProductLines] = useState<IBudgetProductSaveDto[]>(initialForm.productLines);
  const [filterCustomerName, setFilterCustomerName] = useState('');
  const [filterProductDescription, setFilterProductDescription] = useState('');

  const valueTotal = CommercialDocumentFormService.calculateTotal(productLines, discountValue);
  const {
    loadFromPayment: loadInstallmentFromPayment,
    getInstallmentState,
    ...installmentFields
  } = UseDocumentInstallmentForm(valueTotal);

  useEffect(() => {
    loadCustomers({ registersStart: 0, registersEnd: PAGE_SIZE });
    loadProducts({ registersStart: 0, registersEnd: PAGE_SIZE });
    if (isEditing) loadById(id);

    return () => clearCurrentItem();
  }, [id]);

  useEffect(() => {
    if (!currentItem) return;

    const form = BudgetFormService.detailToForm(currentItem);
    setCustomerId(form.customerId);
    setCustomerDisplayLabel(form.customerDisplayLabel);
    setObservations(form.observations);
    setSignature(form.signature);
    setPaymentMethod(form.paymentMethod);
    setDiscountValue(form.discountValue);
    setProductLines(form.productLines);
    loadInstallmentFromPayment(currentItem.payment);
  }, [currentItem]);

  const currentForm = () => ({
    customerId,
    customerDisplayLabel,
    observations,
    signature,
    paymentMethod,
    discountValue,
    productLines,
  });

  const selectCustomer = (customer: ICustomerListing) => {
    setCustomerId(customer.id);
    setCustomerDisplayLabel(customer.name);
    clearError('customerId');
  };

  const updateProductLine = (index: number, field: keyof IBudgetProductSaveDto, value: number) => {
    setProductLines((previous) =>
      CommercialDocumentFormService.updateProductLine(previous, index, field, value),
    );
  };

  const selectProductLine = (index: number, product: { id: number; description: string }) => {
    setProductLines((previous) =>
      CommercialDocumentFormService.selectProductLine(previous, index, product),
    );
  };

  const addProductLine = () => {
    setProductLines((previous) => [
      ...previous,
      CommercialDocumentFormService.createEmptyProductLine<IBudgetProductSaveDto>(),
    ]);
  };

  const removeProductLine = (index: number) => {
    setProductLines((previous) => previous.filter((_, currentIndex) => currentIndex !== index));
  };

  const handleSave = async () => {
    const form = currentForm();
    const isValid = validate(BudgetFormService.buildValidationRules(form));

    if (!isValid) return;

    const success = await save(
      id ?? null,
      BudgetFormService.buildSaveDto(id, form, getInstallmentState()),
    );
    if (success) onGoBack();
  };

  return {
    pageSize: PAGE_SIZE,
    isEditing,
    isLoadingItem,
    isSaving,
    customers,
    isLoadingCustomers,
    loadCustomers,
    products,
    isLoadingProducts,
    loadProducts,
    errors,
    customerId,
    customerDisplayLabel,
    selectCustomer,
    observations,
    setObservations,
    signature,
    setSignature,
    paymentMethod,
    setPaymentMethod,
    paymentMethodOptions: CommercialDocumentFormService.paymentMethodOptions,
    discountValue,
    setDiscountValue,
    ...installmentFields,
    productLines,
    updateProductLine,
    selectProductLine,
    addProductLine,
    removeProductLine,
    valueTotal,
    filterCustomerName,
    setFilterCustomerName,
    filterProductDescription,
    setFilterProductDescription,
    handleSave,
  };
}
