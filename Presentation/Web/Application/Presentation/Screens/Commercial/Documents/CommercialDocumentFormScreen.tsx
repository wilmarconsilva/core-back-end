import { Ionicons } from '@expo/vector-icons';
import React, { ReactNode } from 'react';
import { ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { PaymentMethod } from '../../../../Application/Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IDocumentProductSaveDto } from '../../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/IDocumentProductSaveDto';
import { ICustomerListing } from '../../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { IProductListing } from '../../../../Application/Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../../../../Application/Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { ValidationErrors } from '../../../../Application/Hooks/UseFormValidation';
import { Breadcrumb } from '../../../Components/Breadcrumb';
import { EntitySelectField } from '../../../Components/EntitySelectField';
import { FormField } from '../../../Components/FormField';
import { NumberField } from '../../../Components/NumberField';
import { PrimaryButton } from '../../../Components/PrimaryButton';
import { SecondaryButton } from '../../../Components/SecondaryButton';
import { SelectField } from '../../../Components/SelectField';
import { SignatureCanvas } from '../../../Components/SignatureCanvas';
import { TextAreaField } from '../../../Components/TextAreaField';
import { colors, documentFormScreenStyles as styles, layoutStyles, typographyStyles } from '../../../Styles';

type ProductLineEditableField = 'quantity' | 'valueUnit';

interface CommercialDocumentFormScreenProps {
  title: string;
  breadcrumbEntityLabel: string;
  breadcrumbCurrentLabel: string;
  observationsPlaceholder: string;
  onGoBack: () => void;
  renderGeneralExtraFields?: () => ReactNode;
  pageSize: number;
  isEditing: boolean;
  isLoadingItem: boolean;
  isSaving: boolean;
  customers: ICustomerListing[];
  isLoadingCustomers: boolean;
  loadCustomers: (filter: ICustomerListingFilter) => Promise<void>;
  products: IProductListing[];
  isLoadingProducts: boolean;
  loadProducts: (filter: IProductListingFilter) => Promise<void>;
  errors: ValidationErrors;
  customerId: number;
  customerDisplayLabel: string;
  selectCustomer: (customer: ICustomerListing) => void;
  observations: string;
  setObservations: (value: string) => void;
  setSignature: (value: string | null) => void;
  paymentMethod: PaymentMethod;
  setPaymentMethod: (value: PaymentMethod) => void;
  paymentMethodOptions: { label: string; value: PaymentMethod }[];
  discountValue: string;
  setDiscountValue: (value: string) => void;
  installmentQty: string;
  setInstallmentQty: (value: string) => void;
  productLines: IDocumentProductSaveDto[];
  updateProductLine: (index: number, field: ProductLineEditableField, value: number) => void;
  selectProductLine: (index: number, product: { id: number; description: string }) => void;
  addProductLine: () => void;
  removeProductLine: (index: number) => void;
  valueTotal: number;
  filterCustomerName: string;
  setFilterCustomerName: (value: string) => void;
  filterProductDescription: string;
  setFilterProductDescription: (value: string) => void;
  handleSave: () => Promise<void>;
}

export function CommercialDocumentFormScreen({
  title,
  breadcrumbEntityLabel,
  breadcrumbCurrentLabel,
  observationsPlaceholder,
  onGoBack,
  renderGeneralExtraFields,
  pageSize,
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
  setSignature,
  paymentMethod,
  setPaymentMethod,
  paymentMethodOptions,
  discountValue,
  setDiscountValue,
  installmentQty,
  setInstallmentQty,
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
}: CommercialDocumentFormScreenProps) {
  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={[layoutStyles.scrollContainer, { justifyContent: 'flex-start' }]}>
        <Breadcrumb items={[
          { label: 'Início', onPress: onGoBack },
          { label: breadcrumbEntityLabel, onPress: onGoBack },
          { label: breadcrumbCurrentLabel },
        ]} />
        <Text style={typographyStyles.title}>{title}</Text>

        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Dados Gerais</Text>
          <EntitySelectField<ICustomerListing, ICustomerListingFilter>
            label="Cliente *"
            value={customerId || undefined}
            selectedDisplayLabel={customerDisplayLabel}
            placeholder="Selecione o cliente..."
            error={errors.customerId}
            disabled={isSaving}
            listings={customers}
            isLoading={isLoadingCustomers}
            onLoadData={loadCustomers}
            pageSize={pageSize}
            filterValues={{ name: filterCustomerName }}
            buildFilter={(page, filters) => ({
              registersStart: (page - 1) * pageSize,
              registersEnd: page * pageSize,
              name: filters.name?.trim() || undefined,
            })}
            columns={[
              { key: 'id', label: 'Código', flex: 0.4 },
              { key: 'name', label: 'Nome', flex: 2 },
              { key: 'document', label: 'Documento', flex: 1.2 },
            ]}
            keyExtractor={(customer) => String(customer.id)}
            labelExtractor={(customer) => customer.name}
            onValueChange={selectCustomer}
            renderFilter={(onSubmitSearch) => (
              <FormField
                label="Nome"
                placeholder="Filtrar..."
                value={filterCustomerName}
                onChangeText={setFilterCustomerName}
                onSubmitEditing={onSubmitSearch}
              />
            )}
          />
          {renderGeneralExtraFields?.()}
          <TextAreaField
            label="Observações"
            placeholder={observationsPlaceholder}
            value={observations}
            onChangeText={setObservations}
            editable={!isSaving}
          />
        </View>

        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Pagamento</Text>
          <SelectField
            label="Método de Pagamento *"
            value={paymentMethod}
            options={paymentMethodOptions}
            onValueChange={(value) => setPaymentMethod(value as PaymentMethod)}
            disabled={isSaving}
          />
          <NumberField
            label="Desconto (R$)"
            placeholder="0.00"
            value={discountValue}
            onChangeText={setDiscountValue}
            editable={!isSaving}
          />
          {paymentMethod === PaymentMethod.CreditCard && (
            <NumberField
              label="Parcelas"
              placeholder="Número de parcelas"
              value={installmentQty}
              onChangeText={setInstallmentQty}
              editable={!isSaving}
            />
          )}
        </View>

        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Produtos</Text>
          {productLines.map((line, index) => (
            <View key={index} style={styles.productRow}>
              <View style={styles.productField}>
                <EntitySelectField<IProductListing, IProductListingFilter>
                  label="Produto"
                  value={line.productId || undefined}
                  selectedDisplayLabel={line.productDescription}
                  placeholder="Selecione..."
                  disabled={isSaving}
                  listings={products}
                  isLoading={isLoadingProducts}
                  onLoadData={loadProducts}
                  pageSize={pageSize}
                  filterValues={{ description: filterProductDescription }}
                  buildFilter={(page, filters) => ({
                    registersStart: (page - 1) * pageSize,
                    registersEnd: page * pageSize,
                    description: filters.description?.trim() || undefined,
                  })}
                  columns={[
                    { key: 'id', label: 'Código', flex: 0.4 },
                    { key: 'description', label: 'Descrição', flex: 2 },
                  ]}
                  keyExtractor={(product) => String(product.id)}
                  labelExtractor={(product) => product.description}
                  onValueChange={(product) => selectProductLine(index, product)}
                  renderFilter={(onSubmitSearch) => (
                    <FormField
                      label="Descrição"
                      placeholder="Filtrar..."
                      value={filterProductDescription}
                      onChangeText={setFilterProductDescription}
                      onSubmitEditing={onSubmitSearch}
                    />
                  )}
                />
              </View>
              <View style={styles.productSmallField}>
                <NumberField
                  label="Qtd."
                  value={String(line.quantity)}
                  onChangeText={(value) => updateProductLine(index, 'quantity', parseFloat(value))}
                  editable={!isSaving}
                />
              </View>
              <View style={styles.productSmallField}>
                <NumberField
                  label="Vl. Unit."
                  value={String(line.valueUnit)}
                  onChangeText={(value) => updateProductLine(index, 'valueUnit', parseFloat(value))}
                  editable={!isSaving}
                />
                <Text style={styles.totalText}>
                  Total: {line.valueTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                </Text>
              </View>
              {productLines.length > 1 && (
                <TouchableOpacity onPress={() => removeProductLine(index)} activeOpacity={0.7} style={{ paddingBottom: 12 }}>
                  <Ionicons name="trash-outline" size={18} color={colors.error} />
                </TouchableOpacity>
              )}
            </View>
          ))}
          <TouchableOpacity style={styles.addDocumentProductButton} onPress={addProductLine} activeOpacity={0.7}>
            <Ionicons name="add-outline" size={16} color={colors.primary} />
            <Text style={styles.addButtonText}>Adicionar Produto</Text>
          </TouchableOpacity>
          <View style={styles.summaryRow}>
            <Text style={styles.summaryText}>
              Total: {valueTotal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
            </Text>
          </View>
        </View>

        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Assinatura</Text>
          <SignatureCanvas onSignatureChange={setSignature} width={500} height={180} />
        </View>

        <View style={styles.documentFooterActions}>
          <SecondaryButton title="Cancelar" onPress={onGoBack} style={{ flex: 1 }} disabled={isSaving} />
          <PrimaryButton
            title={breadcrumbCurrentLabel === 'Editar' ? 'Salvar' : 'Criar'}
            isLoading={isSaving || isLoadingItem}
            onPress={handleSave}
            style={{ flex: 1 }}
          />
        </View>
      </ScrollView>
    </View>
  );
}
