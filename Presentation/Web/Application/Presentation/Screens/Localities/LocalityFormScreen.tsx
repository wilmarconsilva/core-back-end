import React, { useEffect, useState } from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { FormField } from '../../Components/FormField';
import { NumberField } from '../../Components/NumberField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { SecondaryButton } from '../../Components/SecondaryButton';
import { UseFormValidation } from '../../../Application/Hooks/UseFormValidation';
import { UseLocalities } from '../../../Application/Hooks/UseLocalities';
import { ILocalitySaveDto } from '../../../Application/Interfaces/DataTransferObjects/Localities/ILocalitySaveDto';
import { IEntityFormScreenProps } from '../../Interfaces/Screens/IEntityFormScreenProps';
import { formScreenCommonStyles as styles, layoutStyles, typographyStyles } from '../../Styles';

export function LocalityFormScreen({ id, onGoBack }: IEntityFormScreenProps) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseLocalities();
  const { errors, validate, bindChange } = UseFormValidation();
  const isEditing = !!id;

  const [description, setDescription] = useState('');
  const [stateId, setStateId] = useState('');

  useEffect(() => {
    if (isEditing) {
      loadById(id);
    }
    return () => clearCurrentItem();
  }, [id]);

  useEffect(() => {
    if (currentItem) {
      setDescription(currentItem.description ?? '');
      const sid = currentItem.state?.id;
      setStateId(sid !== undefined && sid !== null ? String(sid) : '');
    }
  }, [currentItem]);

  const handleSave = async () => {
    const isValid = validate([
      { key: 'description', isValid: !!description.trim() },
    ]);
    if (!isValid) return;

    const dto: ILocalitySaveDto = {
      id: id ?? 0,
      description: description.trim(),
      stateId: stateId.trim() ? parseInt(stateId, 10) : undefined,
    };
    const success = await save(id ?? null, dto);
    if (success) onGoBack();
  };

  const breadcrumbItems = [
    { label: 'Início', onPress: () => onGoBack() },
    { label: 'Localidades', onPress: () => onGoBack() },
    { label: isEditing ? 'Editar' : 'Nova Localidade' },
  ];

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={[layoutStyles.scrollContainer, { justifyContent: 'flex-start' }]}>
        <Breadcrumb items={breadcrumbItems} />
        <Text style={typographyStyles.title}>
          {isEditing ? 'Editar Localidade' : 'Nova Localidade'}
        </Text>

        <View style={styles.entityCard}>
          <FormField
            label="Descrição *"
            placeholder="Nome da localidade"
            value={description}
            onChangeText={bindChange('description', setDescription)}
            editable={!isSaving}
            error={errors.description}
          />
          <NumberField
            label="Estado (código)"
            placeholder="ID do estado (opcional)"
            value={stateId}
            onChangeText={setStateId}
            editable={!isSaving}
          />
          {isEditing && currentItem?.state?.acronym ? (
            <Text style={{ marginBottom: 8, opacity: 0.75 }}>
              Estado selecionado: {currentItem.state.acronym}
              {currentItem.state.description ? ` — ${currentItem.state.description}` : ''}
            </Text>
          ) : null}

          <View style={styles.actionRow}>
            <SecondaryButton title="Cancelar" onPress={onGoBack} style={{ flex: 1 }} disabled={isSaving} />
            <PrimaryButton
              title={isEditing ? 'Salvar' : 'Criar'}
              isLoading={isSaving || isLoadingItem}
              onPress={handleSave}
              style={{ flex: 1 }}
            />
          </View>
        </View>
      </ScrollView>
    </View>
  );
}
