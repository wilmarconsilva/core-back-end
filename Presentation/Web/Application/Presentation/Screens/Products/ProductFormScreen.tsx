import React, { useEffect, useState } from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { FormField } from '../../Components/FormField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { SecondaryButton } from '../../Components/SecondaryButton';
import { SelectField } from '../../Components/SelectField';
import { UseFormValidation } from '../../../Application/Hooks/UseFormValidation';
import { UseProducts } from '../../../Application/Hooks/UseProducts';
import { MeasureUnit, MeasureUnitLabels } from '../../../Application/Domain/ValueObjects/Enums/Products/MeasureUnit';
import { IProductSaveDto } from '../../../Application/Interfaces/DataTransferObjects/Products/IProductSaveDto';
import { IProductFormScreenProps } from '../../Interfaces/Product/IProductFormScreenProps';
import { productFormScreenStyles as styles, layoutStyles, typographyStyles } from '../../Styles';

const measureUnitOptions = Object.values(MeasureUnit)
  .filter((v) => typeof v === 'number')
  .map((v) => ({ label: MeasureUnitLabels[v as MeasureUnit], value: v as MeasureUnit }));

export function ProductFormScreen({ id, onGoBack }: IProductFormScreenProps) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseProducts();
  const { errors, validate, bindChange } = UseFormValidation();
  const isEditing = !!id;

  const [description, setDescription] = useState('');
  const [measureUnit, setMeasureUnit] = useState<MeasureUnit>(MeasureUnit.Meters);

  useEffect(() => {
    if (isEditing) loadById(id);
    return () => clearCurrentItem();
  }, [id]);

  useEffect(() => {
    if (currentItem) {
      setDescription(currentItem.description ?? '');
      setMeasureUnit(currentItem.measureUnit);
    }
  }, [currentItem]);

  const handleSave = async () => {
    const isValid = validate([
      { key: 'description', isValid: !!description.trim() },
    ]);
    if (!isValid) return;

    const dto: IProductSaveDto = {
      id: id ?? 0,
      description: description.trim(),
      measureUnit,
    };
    const success = await save(id ?? null, dto);
    if (success) onGoBack();
  };

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={[layoutStyles.scrollContainer, { justifyContent: 'flex-start' }]}>
        <Breadcrumb items={[
          { label: 'Início', onPress: onGoBack },
          { label: 'Produtos', onPress: onGoBack },
          { label: isEditing ? 'Editar' : 'Novo Produto' },
        ]} />
        <Text style={typographyStyles.title}>{isEditing ? 'Editar Produto' : 'Novo Produto'}</Text>

        <View style={styles.card}>
          <FormField
            label="Descrição *"
            placeholder="Nome do produto"
            value={description}
            onChangeText={bindChange('description', setDescription)}
            editable={!isSaving}
            error={errors.description}
          />
          <SelectField
            label="Unidade de Medida *"
            value={measureUnit}
            options={measureUnitOptions}
            onValueChange={setMeasureUnit}
            disabled={isSaving}
          />
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
