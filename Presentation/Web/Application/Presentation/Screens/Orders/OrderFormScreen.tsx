import { Ionicons } from '@expo/vector-icons';
import React, { useEffect, useState } from 'react';
import { ScrollView, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { NumberField } from '../../Components/NumberField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { SecondaryButton } from '../../Components/SecondaryButton';
import { SelectField } from '../../Components/SelectField';
import { SignatureCanvas } from '../../Components/SignatureCanvas';
import { TextAreaField } from '../../Components/TextAreaField';
import { UseCustomers } from '../../../Application/Hooks/UseCustomers';
import { UseOrders } from '../../../Application/Hooks/UseOrders';
import { UseProducts } from '../../../Application/Hooks/UseProducts';
import { IOrderProductSaveDto } from '../Interfaces/DataTransferObjects/Commercial/Documents/Orders/IOrderProductSaveDto';
import { IOrderSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/Orders/OrderSaveDto';
import { PaymentMethod, PaymentMethodLabels } from '../../../Application/Domain/ValueObjects/Enums/Commercial/Documents/Payments/PaymentMethod';
import { IPaymentSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Commercial/Documents/PaymentSaveDto';
import { colors, layoutStyles, typographyStyles } from '../../Styles';

interface OrderFormScreenProps {
  id?: number;
  onGoBack: () => void;
}

const paymentMethodOptions = Object.values(PaymentMethod)
  .filter((v) => typeof v === 'number')
  .map((v) => ({ label: PaymentMethodLabels[v as PaymentMethod], value: v as PaymentMethod }));

const styles = StyleSheet.create({
  card: {
    backgroundColor: colors.surface,
    borderRadius: 12,
    padding: 24,
    marginBottom: 16,
    shadowColor: colors.shadow,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.06,
    shadowRadius: 4,
    elevation: 2,
  },
  sectionTitle: { fontSize: 16, fontWeight: '700', color: colors.textPrimary, marginBottom: 16 },
  productRow: {
    flexDirection: 'row',
    gap: 8,
    borderWidth: 1,
    borderColor: colors.border,
    borderRadius: 8,
    padding: 12,
    marginBottom: 10,
    backgroundColor: colors.surfaceAlt,
    alignItems: 'flex-end',
  },
  productField: { flex: 1 },
  productSmallField: { width: 90 },
  totalText: { fontSize: 13, color: colors.textSecondary, marginTop: 4 },
  addButton: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
    paddingVertical: 10,
    paddingHorizontal: 16,
    borderWidth: 1.5,
    borderColor: colors.primary,
    borderRadius: 8,
    alignSelf: 'flex-start',
    marginTop: 4,
  },
  addButtonText: { color: colors.primary, fontWeight: '600', fontSize: 14 },
  summaryRow: { flexDirection: 'row', justifyContent: 'flex-end', marginTop: 12 },
  summaryText: { fontSize: 15, fontWeight: '700', color: colors.textPrimary },
  actionRow: { flexDirection: 'row', gap: 12, marginTop: 8, marginBottom: 24 },
  flex1: { flex: 1 },
});

function createEmptyProduct(): IOrderProductSaveDto {
  return { id: 0, productId: 0, quantity: 1, valueUnit: 0, valueTotal: 0 };
}

export function OrderFormScreen({ id, onGoBack }: OrderFormScreenProps) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseOrders();
  const { listings: customers, loadAll: loadCustomers } = UseCustomers();
  const { listings: products, loadAllForSelect: loadProducts } = UseProducts();
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

  useEffect(() => {
    loadCustomers({ registersStart: 0, registersEnd: 50 });
    loadProducts();
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

  const customerOptions = customers.map((c) => ({ label: `${c.name} - ${c.document}`, value: c.id }));
  const productOptions = products.map((p) => ({ label: p.description, value: p.id }));

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
          <SelectField
            label="Cliente *"
            value={customerId || undefined}
            options={customerOptions}
            onValueChange={(v) => setCustomerId(v as number)}
            placeholder="Selecione o cliente..."
            disabled={isSaving}
          />
          <NumberField
            label="Código do Vendedor *"
            placeholder="Código do usuário"
            value={userId}
            onChangeText={setUserId}
            editable={!isSaving}
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
                <SelectField
                  label="Produto"
                  value={line.productId || undefined}
                  options={productOptions}
                  onValueChange={(v) => updateProductLine(index, 'productId', v as number)}
                  placeholder="Selecione..."
                  disabled={isSaving}
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
          <TouchableOpacity style={styles.addButton} onPress={addProductLine} activeOpacity={0.7}>
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

        <View style={styles.actionRow}>
          <SecondaryButton title="Cancelar" onPress={onGoBack} style={{ flex: 1 }} disabled={isSaving} />
          <PrimaryButton
            title={isEditing ? 'Salvar' : 'Criar'}
            isLoading={isSaving || isLoadingItem}
            onPress={handleSave}
            style={{ flex: 1 }}
            disabled={!customerId || !userId.trim()}
          />
        </View>
      </ScrollView>
    </View>
  );
}
