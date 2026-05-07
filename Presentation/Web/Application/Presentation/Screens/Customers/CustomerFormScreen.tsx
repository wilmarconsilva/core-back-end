import { Ionicons } from '@expo/vector-icons';
import React, { useEffect, useState } from 'react';
import { formatWithMask, Masks } from 'react-native-mask-input';
import { ScrollView, Text, TouchableOpacity, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { EntitySelectField } from '../../Components/EntitySelectField';
import { FormField } from '../../Components/FormField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { SecondaryButton } from '../../Components/SecondaryButton';
import { SelectField } from '../../Components/SelectField';
import { UseCustomers } from '../../../Application/Hooks/UseCustomers';
import { UseFormValidation } from '../../../Application/Hooks/UseFormValidation';
import { UseLocalities } from '../../../Application/Hooks/UseLocalities';
import { ILocalityListing } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';
import { ICustomerAddressSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerAddressSaveDto';
import { ICustomerDetailDto } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerDetailDto';
import { ICustomerSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Persons/Customers/ICustomerSaveDto';
import { PersonType, PersonTypeLabels } from '../../../Application/Interfaces/DataTransferObjects/Persons/PersonType';
import { IEntityFormScreenProps } from '../../Interfaces/Screens/IEntityFormScreenProps';
import { colors, customerFormScreenStyles as styles, layoutStyles, typographyStyles } from '../../Styles';
const PAGE_SIZE = 50;

/** Formato aceito: local@domínio.tld (sem espaços; um único @). */
const EMAIL_MASK_REGEX = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;

function sanitizeEmailInput(raw: string): string {
  const noSpaces = raw.replace(/\s/g, '');
  const at = noSpaces.indexOf('@');
  if (at === -1) return noSpaces;
  const local = noSpaces.slice(0, at).replace(/@/g, '');
  const domain = noSpaces.slice(at + 1).replace(/@/g, '');
  return `${local}@${domain}`;
}

const personTypeOptions = Object.values(PersonType)
  .filter((v) => typeof v === 'number')
  .map((v) => ({ label: PersonTypeLabels[v as PersonType], value: v as PersonType }));

function createEmptyAddress(): ICustomerAddressSaveDto {
  return { id: 0, neighborhood: '', street: '', localityId: 0 };
}

function mapDetailAddressesToForm(detail: ICustomerDetailDto): ICustomerAddressSaveDto[] {
  return detail.addresses.map((a) => ({
    id: a.id,
    neighborhood: a.neighborhood,
    street: a.street,
    localityId: a.locality.id,
    localityDescription: a.locality.description,
    complement: a.complement,
    postalCode: a.postalCode,
    number: a.number,
  }));
}

function validateDocument(personType: PersonType, document: string): boolean {
  if (personType === PersonType.Natural) {
    return document.length === 14;
  }
  else if(personType === PersonType.Legal) {
    return document.length === 18;
  }

  return false;
}

const addressErrorKey = (index: number, field: keyof ICustomerAddressSaveDto) =>
  `addresses.${index}.${field}`;

export function CustomerFormScreen({ id, onGoBack }: IEntityFormScreenProps) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseCustomers();
  const { listings: localities, isLoadingList: isLoadingLocalities, loadAll: loadLocalities } = UseLocalities();
  const { errors, validate, clearError, clearErrors, bindChange } = UseFormValidation();
  const isEditing = !!id;

  const [name, setName] = useState('');
  const [personType, setPersonType] = useState<PersonType>(PersonType.Natural);
  const [document, setDocument] = useState('');
  const [email, setEmail] = useState('');
  const [stateRegistration, setStateRegistration] = useState('');
  const [addresses, setAddresses] = useState<ICustomerAddressSaveDto[]>([createEmptyAddress()]);

  const [filterLocalityDescription, setFilterLocalityDescription] = useState('');

  const documentFieldLabel = personType === PersonType.Natural ? 'CPF' : 'CNPJ';
  const documentFieldPlaceholder = personType === PersonType.Natural ? '000.000.000-00' : 'CNPJ Alfanumérico';
  const documentMask = personType === PersonType.Natural ? Masks.BRL_CPF : null;

  useEffect(() => {
    loadLocalities({ registersStart: 0, registersEnd: PAGE_SIZE });
    if (isEditing) loadById(id);
    return () => clearCurrentItem();
  }, [id]);

  useEffect(() => {
    if (currentItem) {
      setName(currentItem.name ?? '');
      setPersonType(currentItem.personType);
      if (currentItem.personType === PersonType.Natural) {
        setDocument(formatWithMask({ text: currentItem.document ?? '', mask: Masks.BRL_CPF }).masked);
      } else {
        setDocument(currentItem.document ?? '');
      }
      setEmail(sanitizeEmailInput(currentItem.email ?? ''));
      setStateRegistration(currentItem.stateRegistration ?? '');
      setAddresses(
        currentItem.addresses?.length
          ? mapDetailAddressesToForm(currentItem as ICustomerDetailDto)
          : [createEmptyAddress()],
      );
    }
  }, [currentItem]);

  useEffect(() => {
    if (personType === PersonType.Natural) {
      setDocument((prev) =>
        formatWithMask({ text: prev.replace(/\D/g, ''), mask: Masks.BRL_CPF }).masked,
      );
    }
  }, [personType]);

  const updateAddress = (index: number, field: keyof ICustomerAddressSaveDto, value: string | number) => {
    setAddresses(prev => prev.map((a, i) => i === index ? { ...a, [field]: value } : a));
    clearError(addressErrorKey(index, field));
  };

  const addAddress = () => {
    setAddresses(prev => [...prev, createEmptyAddress()]);
    clearErrors();
  };
  const removeAddress = (index: number) => {
    setAddresses(prev => prev.filter((_, i) => i !== index));
    clearErrors();
  };

  const handleSave = async () => {
    const emailTrimmed = email.trim();
    const isValid = validate([
      { key: 'name', isValid: !!name.trim() },
      { key: 'document', isValid: !!document.trim() },
      {
        key: 'email',
        isValid: !emailTrimmed || EMAIL_MASK_REGEX.test(emailTrimmed),
        message: 'Informe um e-mail válido.',
      },
      { key: 'document', isValid: validateDocument(personType, document), message: `Informe um ${documentFieldLabel} válido.` },
      ...addresses.flatMap((address, index) => [
        { key: addressErrorKey(index, 'street'), isValid: !!address.street.trim() },
        { key: addressErrorKey(index, 'neighborhood'), isValid: !!address.neighborhood.trim() },
        { key: addressErrorKey(index, 'localityId'), isValid: !!address.localityId },
      ]),
    ]);
    if (!isValid) return;

    const dto: ICustomerSaveDto = {
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
          <FormField label="Nome *" placeholder="Nome completo" value={name} onChangeText={bindChange('name', setName)} editable={!isSaving} error={errors.name} />
          <SelectField
            label="Tipo de Pessoa *"
            value={personType}
            options={personTypeOptions}
            onValueChange={setPersonType}
            disabled={isSaving}
          />
          <FormField
            label={`${documentFieldLabel} *`}
            placeholder={documentFieldPlaceholder}
            mask={documentMask ?? undefined}
            value={document}
            onChangeText={bindChange('document', setDocument)}
            editable={!isSaving}
            error={errors.document}
          />
          <FormField
            label="E-mail"
            placeholder="email@exemplo.com"
            value={email}
            onChangeText={(text) => bindChange('email', setEmail)(sanitizeEmailInput(text))}
            editable={!isSaving}
            keyboardType="email-address"
            error={errors.email}
          />
          <FormField
            label="Inscrição Estadual"
            value={stateRegistration}
            onChangeText={setStateRegistration}
            editable={!isSaving}
          />
        </View>

        {/* Addresses */}
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Endereço{/* Descomentar quando adaptar orçamentos e pedidos para mais de um endereço: s */}</Text>
          {addresses.map((addr, index) => (
            <View key={index} style={styles.addressCard}>
              <View style={styles.addressHeader}>
                {/* Descomentar quando adaptar orçamentos e pedidos para mais de um endereço: <Text style={styles.addressTitle}>Endereço {index + 1}</Text> */}
                {addresses.length > 1 && (
                  <TouchableOpacity onPress={() => removeAddress(index)} activeOpacity={0.7}>
                    <Ionicons name="trash-outline" size={18} color={colors.error} />
                  </TouchableOpacity>
                )}
              </View>
              <View style={styles.row}>
                <View style={styles.flex1}>
                  <FormField label="Rua *" placeholder="Nome da rua" value={addr.street} onChangeText={(v) => updateAddress(index, 'street', v)} editable={!isSaving} error={errors[addressErrorKey(index, 'street')]} />
                </View>
                <View style={styles.flex1}>
                  <FormField label="Bairro *" placeholder="Nome do bairro" value={addr.neighborhood} onChangeText={(v) => updateAddress(index, 'neighborhood', v)} editable={!isSaving} error={errors[addressErrorKey(index, 'neighborhood')]} />
                </View>
              </View>
              <EntitySelectField<ILocalityListing, ILocalityListingFilter>
                label="Localidade *"
                value={addr.localityId || undefined}
                selectedDisplayLabel={addr.localityDescription}
                placeholder="Selecione a localidade..."
                disabled={isSaving}
                error={errors[addressErrorKey(index, 'localityId')]}
                listings={localities}
                isLoading={isLoadingLocalities}
                onLoadData={loadLocalities}
                pageSize={PAGE_SIZE}
                filterValues={{ description: filterLocalityDescription }}
                buildFilter={(page, filters) => ({
                  registersStart: (page - 1) * PAGE_SIZE,
                  registersEnd: page * PAGE_SIZE,
                  description: filters.description?.trim() || undefined,
                })}
                columns={[
                  { key: 'id', label: 'Código', flex: 0.4 },
                  { key: 'description', label: 'Descrição', flex: 2 },
                ]}
                keyExtractor={(l) => String(l.id)}
                labelExtractor={(l) => l.description}
                onValueChange={(locality) => {
                  setAddresses(prev =>
                    prev.map((a, i) =>
                      i === index
                        ? { ...a, localityId: locality.id, localityDescription: locality.description }
                        : a,
                    ),
                  );
                  clearError(addressErrorKey(index, 'localityId'));
                }}
                renderFilter={(onSubmitSearch) => (
                  <FormField
                    label="Descrição"
                    placeholder="Filtrar..."
                    value={filterLocalityDescription}
                    onChangeText={setFilterLocalityDescription}
                    onSubmitEditing={onSubmitSearch}
                  />
                )}
              />
              <View style={styles.row}>
                <View style={styles.flex1}>
                  <FormField label="Número" placeholder="Nº" value={addr.number ?? ''} onChangeText={(v) => updateAddress(index, 'number', v)} editable={!isSaving} />
                </View>
                <View style={styles.flex1}>
                  <FormField 
                  label="CEP"
                  placeholder="00000-000" 
                  value={addr.postalCode ?? ''} 
                  onChangeText={(v) => updateAddress(index, 'postalCode', v)} 
                  editable={!isSaving}
                  mask={Masks.ZIP_CODE} />
                </View>
              </View>
              <FormField label="Complemento" placeholder="Apt, bloco, etc." value={addr.complement ?? ''} onChangeText={(v) => updateAddress(index, 'complement', v)} editable={!isSaving} />
            </View>
          ))}
{/*Descomentar quando adaptar orçamentos e pedidos para mais de um endereço: <TouchableOpacity style={styles.addCustomerAddressButton} onPress={addAddress} activeOpacity={0.7}>
            <Ionicons name="add-outline" size={16} color={colors.primary} />
            <Text style={styles.addButtonText}>Adicionar endereço</Text>
          </TouchableOpacity> */}
        </View>

        <View style={styles.actionRow}>
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
