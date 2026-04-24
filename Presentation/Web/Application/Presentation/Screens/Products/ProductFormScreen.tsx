import React, { useEffect, useState } from 'react';
import { ScrollView, StyleSheet, Text, View } from 'react-native';
import { Breadcrumb } from '../../Components/Breadcrumb';
import { FormField } from '../../Components/FormField';
import { PrimaryButton } from '../../Components/PrimaryButton';
import { SecondaryButton } from '../../Components/SecondaryButton';
import { SelectField } from '../../Components/SelectField';
import { UseProducts } from '../../../Application/Hooks/UseProducts';
import { MeasureUnit, MeasureUnitLabels } from '../../../Application/Interfaces/DataTransferObjects/MeasureUnit';
import { ProductSaveDto } from '../../../Application/Interfaces/DataTransferObjects/ProductSaveDto';
import { colors, layoutStyles, typographyStyles } from '../../Styles';

interface ProductFormScreenProps {
  id?: number;
  onGoBack: () => void;
}

const styles = StyleSheet.create({
  card: {
    backgroundColor: colors.surface,
    borderRadius: 12,
    padding: 24,
    shadowColor: colors.shadow,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.06,
    shadowRadius: 4,
    elevation: 2,
  },
  actionRow: { flexDirection: 'row', gap: 12, marginTop: 8 },
});

const measureUnitOptions = Object.values(MeasureUnit)
  .filter((v) => typeof v === 'number')
  .map((v) => ({ label: MeasureUnitLabels[v as MeasureUnit], value: v as MeasureUnit }));

export function ProductFormScreen({ id, onGoBack }: ProductFormScreenProps) {
  const { currentItem, isLoadingItem, isSaving, loadById, save, clearCurrentItem } = UseProducts();
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
    const dto: ProductSaveDto = {
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
            onChangeText={setDescription}
            editable={!isSaving}
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
              disabled={!description.trim()}
            />
          </View>
        </View>
      </ScrollView>
    </View>
  );
}
