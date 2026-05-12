import React from 'react';
import { Text } from 'react-native';
import { FormField } from '../../Components/FormField';
import { NumberField } from '../../Components/NumberField';
import { UseLocalityForm } from '../../../Application/Hooks/UseLocalityForm';
import { IEntityFormScreenProps } from '../../Interfaces/Screens/IEntityFormScreenProps';
import { CrudFormScreen } from '../CrudFormScreen';

export function LocalityFormScreen({ id, onGoBack }: IEntityFormScreenProps) {
  const {
    currentItem,
    isEditing,
    isLoadingItem,
    isSaving,
    errors,
    description,
    setDescription,
    stateId,
    setStateId,
    handleSave,
  } = UseLocalityForm(id, onGoBack);

  return (
    <CrudFormScreen
      title={isEditing ? 'Editar Localidade' : 'Nova Localidade'}
      breadcrumbEntityLabel="Localidades"
      breadcrumbCurrentLabel={isEditing ? 'Editar' : 'Nova Localidade'}
      isEditing={isEditing}
      isLoadingItem={isLoadingItem}
      isSaving={isSaving}
      onGoBack={onGoBack}
      onSave={handleSave}
    >
      <FormField
        label="Descrição *"
        placeholder="Nome da localidade"
        value={description}
        onChangeText={setDescription}
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
      {isEditing && currentItem?.state?.description ? (
        <Text style={{ marginBottom: 8, opacity: 0.75 }}>
          Estado selecionado: {currentItem.state.description}
        </Text>
      ) : null}
    </CrudFormScreen>
  );
}
