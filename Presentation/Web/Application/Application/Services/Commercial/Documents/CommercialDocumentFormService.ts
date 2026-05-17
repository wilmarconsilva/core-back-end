import { PaymentMethod, PaymentMethodLabels } from '../../../Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IDocumentProductSaveDto } from '../../../Interfaces/DataTransferObjects/Commercial/Documents/IDocumentProductSaveDto';
import { IPaymentSaveDto } from '../../../Interfaces/DataTransferObjects/Commercial/Documents/Payments/IPaymentSaveDto';
import { IEntityReferenceDto } from '../../../Interfaces/DataTransferObjects/Entities/IEntityReferenceDto';
import { DocumentInstallmentFormState, InstallmentFormService } from './Payments/InstallmentFormService';
import { parseCurrencyInput } from '../../Utilities/CurrencyInputUtilities';

interface DocumentProductDetail {
  id: number;
  product: {
    id: number;
    description?: string;
  };
  quantity: number;
  valueUnit: number;
  valueTotal: number;
}

export class CommercialDocumentFormService {
  static readonly paymentMethodOptions = Object.values(PaymentMethod)
    .filter((v) => typeof v === 'number')
    .map((v) => ({ label: PaymentMethodLabels[v as PaymentMethod], value: v as PaymentMethod }));

  static entitySelectLabel(entity: IEntityReferenceDto): string {
    return entity.description;
  }

  static createEmptyProductLine<TProduct extends IDocumentProductSaveDto>(): TProduct {
    return {
      id: 0,
      productId: 0,
      quantity: 1,
      valueUnit: 0,
      valueTotal: 0,
    } as TProduct;
  }

  static detailProductsToForm<TProduct extends IDocumentProductSaveDto>(
    products: DocumentProductDetail[] | undefined,
  ): TProduct[] {
    return products?.length
      ? products.map((product) => ({
        id: product.id,
        productId: product.product.id,
        productDescription: product.product.description,
        quantity: product.quantity,
        valueUnit: product.valueUnit,
        valueTotal: product.valueTotal,
      } as TProduct))
      : [CommercialDocumentFormService.createEmptyProductLine<TProduct>()];
  }

  static updateProductLine<TProduct extends IDocumentProductSaveDto>(
    productLines: TProduct[],
    index: number,
    field: keyof TProduct,
    value: number,
  ): TProduct[] {
    return productLines.map((productLine, currentIndex) => {
      if (currentIndex !== index) return productLine;

      const updated = { ...productLine, [field]: value };

      if (field === 'quantity' || field === 'valueUnit') {
        updated.valueTotal = updated.quantity * updated.valueUnit;
      }

      return updated;
    });
  }

  static selectProductLine<TProduct extends IDocumentProductSaveDto>(
    productLines: TProduct[],
    index: number,
    product: { id: number; description: string },
  ): TProduct[] {
    return productLines.map((productLine, currentIndex) =>
      currentIndex === index
        ? { ...productLine, productId: product.id, productDescription: product.description }
        : productLine,
    );
  }

  static buildPayment(
    paymentMethod: PaymentMethod,
    discountValue: string,
    installment: DocumentInstallmentFormState,
  ): IPaymentSaveDto {
    const discount = parseCurrencyInput(discountValue);

    return {
      method: paymentMethod,
      discountValue: discount,
      installment: InstallmentFormService.toInstallmentSaveDto(installment),
    };
  }

  static calculateTotal(productLines: IDocumentProductSaveDto[], discountValue: string): number {
    const valueSubtotal = productLines.reduce((sum, productLine) => sum + productLine.valueTotal, 0);
    const discount = parseCurrencyInput(discountValue);

    return Math.max(0, valueSubtotal - discount);
  }

  static saveProductLines<TProduct extends IDocumentProductSaveDto>(productLines: TProduct[]): TProduct[] {
    return productLines
      .filter((productLine) => productLine.productId > 0)
      .map((productLine) => {
        const { productDescription: _productDescription, ...saveProductLine } = productLine;

        return saveProductLine as TProduct;
      });
  }
}
