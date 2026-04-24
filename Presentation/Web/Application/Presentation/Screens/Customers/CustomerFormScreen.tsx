import { Ionicons } from '@expo/vector-icons';
import React, { useEffect, useState } from 'react';
import { ScrollView, StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { FormField } from '../../Components/FormField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { SecondaryButton } from '../../Components/SecondaryButton';
import { SelectField } from '../../Components/SelectField';
import { UseCustomers } from '../../../Application/Hooks/UseCustomers';
import { UseLocalities } from '../../../Application/Hooks/UseLocalities';
import { CustomerAddressSaveDto } from '../../../Application/Interfaces/DataTransferObjects/CustomerAddressSaveDto';
import { CustomerSaveDto } from '../../../Application/Interfaces/DataTransferObjects/CustomerSaveDto';
import { PersonType, PersonTypeLabels } from '../../../Application/Interfaces/DataTransferObjects/PersonType';
import { colors, layoutStyles, typographyStyles } from '../../Styles';

interface CustomerFormScreenProps {
  id?: number;
  onGoBack: () => void;
}

const personTypeOptions = Object.values(PersonType)
  .filter((v) => typeof v === 'number')
  .map((v) => ({ label: PersonTypeLabels[v as PersonType], value: v as PersonType }));

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
  sectionTitle: {
    fontSize: 16,
    fontWeight: '700',
    color: colors.textPrimary,
    marginBottom: 16,
  },
  addressCard: {
    borderWidth: 1,
    borderColor: colors.border,
    borderRadius: 8,
    padding: 16,
    marginBottom: 12,
    backgroundColor: colors.surfaceAlt,
  },
  addressHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  addressTitle: {
    fontSize: 14,
    fontWeight: '600',
    color: colors.textPrimary,
  },
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
    marginBottom: 16,
  },
  addButtonText: {
    color: colors.primary,
    fontWeight: '600',
    fontSize: 14,
  },
  actionRow: { flexDirection: 'row', gap: 12, marginTop: 8 },
  row: { flexDirection: 'row', gap: 12 },
  flex1: { flex: 1 },
});

function createEmptyAddress(): CustomerAddressSaveDto {
  return { id: 0, neighborhood: '', street: '', localityId: 0 };
}

export function CustomerFormScreen({ id, onGoBack }: CustomerFormScreenProps) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseCustomers();
  const { listings: localities, loadAllForSelect } = UseLocalities();
  const isEditing = !!id;

  const [name, setName] = useState('');
  const [personType, setPersonType] = useState<PersonType>(PersonType.Natural);
  const [document, setDocument] = useState('');
  const [email, setEmail] = useState('');
  const [stateRegistration, setStateRegistration] = useState('');
  const [addresses, setAddresses] = useState<CustomerAddressSaveDto[]>([createEmptyAddress()]);

  useEffect(() => {
    loadAllForSelect();
    if (isEditing) loadById(id);
    return () => clearCurrentItem();
  }, [id]);

  useEffect(() => {
    if (currentItem) {
      setName(currentItem.name ?? '');
      setPersonType(currentItem.personType);
      setDocument(currentItem.document ?? '');
      setEmail(currentItem.email ?? '');
      setStateRegistration(currentItem.stateRegistration ?? '');
      setAddresses(currentItem.addresses?.length ? currentItem.addresses : [createEmptyAddress()]);
    }
  }, [currentItem]);

  const localityOptions = localities.map((l) => ({ label: l.description, value: l.id }));

  const updateAddress = (index: number, field: keyof CustomerAddressSaveDto, value: string | number) => {
    setAddresses(prev => prev.map((a, i) => i === index ? { ...a, [field]: value } : a));
  };

  const addAddress = () => setAddresses(prev => [...prev, createEmptyAddress()]);
  const removeAddress = (index: number) => setAddresses(prev => prev.filter((_, i) => i !== index));

  const handleSave = async () => {
    const dto: CustomerSaveDto = {
      id: id ?? 0,
      name: name.trim(),
      personType,
      document: document.trim(),
      addresses,
      email: email.trim() || undefined,
      stateRegistration: stateRegistration.trim() || undefined,
    };
    const success = await save(id ?? null, dto);
    if (success) onGoBack();
  };

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={[layoutStyles.scrollContainer, { justifyContent: 'flex-start' }]}>
        <Breadcrumb items={[
          { label: 'Início', onPress: onGoBack },
          { label: 'Clientes', onPress: onGoBack },
          { label: isEditing ? 'Editar' : 'Novo Cliente' },
        ]} />
        <Text style={typographyStyles.title}>{isEditing ? 'Editar Cliente' : 'Novo Cliente'}</Text>

        {/* Main data */}
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Dados Gerais</Text>
          <FormField label="Nome *" placeholder="Nome completo" value={name} onChangeText={setName} editable={!isSaving} />
          <SelectField
            label="Tipo de Pessoa *"
            value={personType}
            options={personTypeOptions}
            onValueChange={setPersonType}
            disabled={isSaving}
          />
          <FormField label="Documento *" placeholder="CPF ou CNPJ" value={document} onChangeText={setDocument} editable={!isSaving} />
          <FormField label="E-mail" placeholder="email@exemplo.com" value={email} onChangeText={setEmail} editable={!isSaving} keyboardType="email-address" />
          <FormField label="Inscrição Estadual" placeholder="Opcional" value={stateRegistration} onChangeText={setStateRegistration} editable={!isSaving} />
        </View>

        {/* Addresses */}
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Endereços</Text>
          {addresses.map((addr, index) => (
            <View key={index} style={styles.addressCard}>
              <View style={styles.addressHeader}>
                <Text style={styles.addressTitle}>Endereço {index + 1}</Text>
                {addresses.length > 1 && (
                  <TouchableOpacity onPress={() => removeAddress(index)} activeOpacity={0.7}>
                    <Ionicons name="trash-outline" size={18} color={colors.error} />
                  </TouchableOpacity>
                )}
              </View>
              <View style={styles.row}>
                <View style={styles.flex1}>
                  <FormField label="Rua *" placeholder="Nome da rua" value={addr.street} onChangeText={(v) => updateAddress(index, 'street', v)} editable={!isSaving} />
                </View>
                <View style={styles.flex1}>
                  <FormField label="Bairro *" placeholder="Nome do bairro" value={addr.neighborhood} onChangeText={(v) => updateAddress(index, 'neighborhood', v)} editable={!isSaving} />
                </View>
              </View>
              <SelectField
                label="Localidade *"
                value={addr.localityId || undefined}
                options={localityOptions}
                onValueChange={(v) => updateAddress(index, 'localityId', v as number)}
                placeholder="Selecione a localidade..."
                disabled={isSaving}
              />
              <View style={styles.row}>
                <View style={styles.flex1}>
                  <FormField label="Número" placeholder="Nº" value={addr.number ?? ''} onChangeText={(v) => updateAddress(index, 'number', v)} editable={!isSaving} />
                </View>
                <View style={styles.flex1}>
                  <FormField label="CEP" placeholder="00000-000" value={addr.postalCode ?? ''} onChangeText={(v) => updateAddress(index, 'postalCode', v)} editable={!isSaving} />
                </View>
              </View>
              <FormField label="Complemento" placeholder="Apt, bloco, etc." value={addr.complement ?? ''} onChangeText={(v) => updateAddress(index, 'complement', v)} editable={!isSaving} />
            </View>
          ))}
          <TouchableOpacity style={styles.addButton} onPress={addAddress} activeOpacity={0.7}>
            <Ionicons name="add-outline" size={16} color={colors.primary} />
            <Text style={styles.addButtonText}>Adicionar Endereço</Text>
          </TouchableOpacity>
        </View>

        <View style={styles.actionRow}>
          <SecondaryButton title="Cancelar" onPress={onGoBack} style={{ flex: 1 }} disabled={isSaving} />
          <PrimaryButton
            title={isEditing ? 'Salvar' : 'Criar'}
            isLoading={isSaving || isLoadingItem}
            onPress={handleSave}
            style={{ flex: 1 }}
            disabled={!name.trim() || !document.trim()}
          />
        </View>
      </ScrollView>
    </View>
  );
}
