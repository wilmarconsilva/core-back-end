import { Ionicons } from '@expo/vector-icons';
import React, { useEffect, useState } from 'react';
import { ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { EntitySelectField } from '../../Components/EntitySelectField';
import { NumberField } from '../../Components/NumberField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { SecondaryButton } from '../../Components/SecondaryButton';
import { SelectField } from '../../Components/SelectField';
import { SignatureCanvas } from '../../Components/SignatureCanvas';
import { TextAreaField } from '../../Components/TextAreaField';
import { FormField } from '../../Components/FormField';
import { UseCustomers } from '../../../Application/Hooks/UseCustomers';
import { UseFormValidation } from '../../../Application/Hooks/UseFormValidation';
import { UseOrders } from '../../../Application/Hooks/UseOrders';
import { UseProducts } from '../../../Application/Hooks/UseProducts';
import { ICustomerListing } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListing';
import { ICustomerListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerListingFilter';
import { IProductListing } from '../../../Application/Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { IOrderProductSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderProductSaveDto';
import { IOrderSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderSaveDto';
import { PaymentMethod, PaymentMethodLabels } from '../../../Application/Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IPaymentSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Payments/IPaymentSaveDto';
import { IEntityFormScreenProps } from '../../Interfaces/Screens/IEntityFormScreenProps';
import { colors, documentFormScreenStyles as styles, layoutStyles, typographyStyles } from '../../Styles';

const PAGE_SIZE = 50;

const paymentMethodOptions = Object.values(PaymentMethod)
  .filter((v) => typeof v === 'number')
  .map((v) => ({ label: PaymentMethodLabels[v as PaymentMethod], value: v as PaymentMethod }));

function createEmptyProduct(): IOrderProductSaveDto {
  return { id: 0, productId: 0, quantity: 1, valueUnit: 0, valueTotal: 0 };
}

export function OrderFormScreen({ id, onGoBack }: IEntityFormScreenProps) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseOrders();
  const { listings: customers, isLoadingList: isLoadingCustomers, loadAll: loadCustomers } = UseCustomers();
  const { listings: products, isLoadingList: isLoadingProducts, loadAll: loadProducts } = UseProducts();
  const { errors, validate, clearError, bindChange } = UseFormValidation();
  const isEditing = !!id;

  const [customerId, setCustomerId] = useState<number>(0);
  const [userId, setUserId] = useState('');
  const [budgetId, setBudgetId] = useState('');
  const [observations, setObservations] = useState('');
  const [signature, setSignature] = useState<string | null>(null);
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(PaymentMethod.Money);
  const [discountValue, setDiscountValue] = useState('0');
  const [installmentQty, setInstallmentQty] = useState('');
  const [productLines, setProductLines] = useState<IOrderProductSaveDto[]>([createEmptyProduct()]);

  const [filterCustomerName, setFilterCustomerName] = useState('');
  const [filterProductDescription, setFilterProductDescription] = useState('');

  useEffect(() => {
    loadCustomers({ registersStart: 0, registersEnd: PAGE_SIZE });
    loadProducts({ registersStart: 0, registersEnd: PAGE_SIZE });
    if (isEditing) loadById(id);
    return () => clearCurrentItem();
  }, [id]);

  useEffect(() => {
    if (currentItem) {
      setCustomerId(currentItem.customerId);
      setUserId(String(currentItem.userId));
      setBudgetId(currentItem.budgetId ? String(currentItem.budgetId) : '');
      setObservations(currentItem.observations ?? '');
      setSignature(currentItem.customerSignatureImage ?? null);
      setPaymentMethod(currentItem.payment?.method ?? PaymentMethod.Money);
      setDiscountValue(String(currentItem.payment?.discountValue ?? 0));
      setInstallmentQty(String(currentItem.payment?.installment?.quantity ?? ''));
      setProductLines(currentItem.products?.length ? currentItem.products : [createEmptyProduct()]);
    }
  }, [currentItem]);

  const updateProductLine = (index: number, field: keyof IOrderProductSaveDto, value: number) => {
    setProductLines(prev => prev.map((p, i) => {
      if (i !== index) return p;
      const updated = { ...p, [field]: value };
      if (field === 'quantity' || field === 'valueUnit') {
        updated.valueTotal = updated.quantity * updated.valueUnit;
      }
      return updated;
    }));
  };

  const addProductLine = () => setProductLines(prev => [...prev, createEmptyProduct()]);
  const removeProductLine = (index: number) => setProductLines(prev => prev.filter((_, i) => i !== index));

  const valueSubtotal = productLines.reduce((sum, p) => sum + p.valueTotal, 0);
  const discount = parseFloat(discountValue) || 0;
  const valueTotal = Math.max(0, valueSubtotal - discount);

  const buildPayment = (): IPaymentSaveDto => ({
    method: paymentMethod,
    discountValue: discount,
    installment: paymentMethod === PaymentMethod.CreditCard && installmentQty
      ? { quantity: parseInt(installmentQty) || 1 }
      : undefined,
  });

  const handleSave = async () => {
    const isValid = validate([
      { key: 'customerId', isValid: !!customerId },
      { key: 'userId', isValid: !!userId.trim() },
    ]);
    if (!isValid) return;

    const dto: IOrderSaveDto = {
      id: id ?? 0,
      userId: parseInt(userId) || 0,
      customerId,
      budgetId: budgetId ? parseInt(budgetId) : undefined,
      payment: buildPayment(),
      observations: observations.trim() || undefined,
      customerSignatureImage: signature ?? undefined,
      products: productLines.filter(p => p.productId > 0),
    };
    const success = await save(id ?? null, dto);
    if (success) onGoBack();
  };

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={[layoutStyles.scrollContainer, { justifyContent: 'flex-start' }]}>
        <Breadcrumb items={[
          { label: 'Início', onPress: onGoBack },
          { label: 'Pedidos', onPress: onGoBack },
          { label: isEditing ? 'Editar' : 'Novo Pedido' },
        ]} />
        <Text style={typographyStyles.title}>{isEditing ? 'Editar Pedido' : 'Novo Pedido'}</Text>

        {/* General */}
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Dados Gerais</Text>
          <EntitySelectField<ICustomerListing, ICustomerListingFilter>
            label="Cliente *"
            value={customerId || undefined}
            placeholder="Selecione o cliente..."
            error={errors.customerId}
            disabled={isSaving}
            listings={customers}
            isLoading={isLoadingCustomers}
            onLoadData={loadCustomers}
            pageSize={PAGE_SIZE}
            filterValues={{ name: filterCustomerName }}
            buildFilter={(page, filters) => ({
              registersStart: (page - 1) * PAGE_SIZE,
              registersEnd: page * PAGE_SIZE,
              name: filters.name?.trim() || undefined,
            })}
            columns={[
              { key: 'id', label: 'Código', flex: 0.4 },
              { key: 'name', label: 'Nome', flex: 2 },
              { key: 'document', label: 'Documento', flex: 1.2 },
            ]}
            keyExtractor={(c) => String(c.id)}
            labelExtractor={(c) => `${c.name} - ${c.document}`}
            onValueChange={(customer) => {
              setCustomerId(customer.id);
              clearError('customerId');
            }}
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
          <NumberField
            label="Código do Vendedor *"
            placeholder="Código do usuário"
            value={userId}
            onChangeText={bindChange('userId', setUserId)}
            editable={!isSaving}
            error={errors.userId}
          />
          <NumberField
            label="Código do Orçamento"
            placeholder="Opcional"
            value={budgetId}
            onChangeText={setBudgetId}
            editable={!isSaving}
          />
          <TextAreaField
            label="Observações"
            placeholder="Observações do pedido..."
            value={observations}
            onChangeText={setObservations}
            editable={!isSaving}
          />
        </View>

        {/* Payment */}
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Pagamento</Text>
          <SelectField
            label="Método de Pagamento *"
            value={paymentMethod}
            options={paymentMethodOptions}
            onValueChange={(v) => setPaymentMethod(v as PaymentMethod)}
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

        {/* Products */}
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Produtos</Text>
          {productLines.map((line, index) => (
            <View key={index} style={styles.productRow}>
              <View style={styles.productField}>
                <EntitySelectField<IProductListing, IProductListingFilter>
                  label="Produto"
                  value={line.productId || undefined}
                  placeholder="Selecione..."
                  disabled={isSaving}
                  listings={products}
                  isLoading={isLoadingProducts}
                  onLoadData={loadProducts}
                  pageSize={PAGE_SIZE}
                  filterValues={{ description: filterProductDescription }}
                  buildFilter={(page, filters) => ({
                    registersStart: (page - 1) * PAGE_SIZE,
                    registersEnd: page * PAGE_SIZE,
                    description: filters.description?.trim() || undefined,
                  })}
                  columns={[
                    { key: 'id', label: 'Código', flex: 0.4 },
                    { key: 'description', label: 'Descrição', flex: 2 },
                  ]}
                  keyExtractor={(p) => String(p.id)}
                  labelExtractor={(p) => p.description}
                  onValueChange={(product) => updateProductLine(index, 'productId', product.id)}
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
                  label="Qtd"
                  value={String(line.quantity)}
                  onChangeText={(v) => updateProductLine(index, 'quantity', parseFloat(v) || 0)}
                  editable={!isSaving}
                />
              </View>
              <View style={styles.productSmallField}>
                <NumberField
                  label="Vl. Unit."
                  value={String(line.valueUnit)}
                  onChangeText={(v) => updateProductLine(index, 'valueUnit', parseFloat(v) || 0)}
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

        {/* Signature */}
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Assinatura</Text>
          <SignatureCanvas onSignatureChange={setSignature} width={500} height={180} />
        </View>

        <View style={styles.documentFooterActions}>
          <SecondaryButton title="Cancelar" onPress={onGoBack} style={{ flex: 1 }} disabled={isSaving} />
          <PrimaryButton
            title={isEditing ? 'Salvar' : 'Criar'}
            isLoading={isSaving || isLoadingItem}
            onPress={handleSave}
            style={{ flex: 1 }}
          />
        </View>
      </ScrollView>
    </View>
  );
}
