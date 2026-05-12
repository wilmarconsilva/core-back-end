import React from 'react';
import { UseBudgetForm } from '../../../../../Application/Hooks/UseBudgetForm';
import { IEntityFormScreenProps } from '../../../../Interfaces/Screens/IEntityFormScreenProps';
import { CommercialDocumentFormScreen } from '../CommercialDocumentFormScreen';

export function BudgetFormScreen({ id, onGoBack }: IEntityFormScreenProps) {
  const form = UseBudgetForm(id, onGoBack);

  return (
    <CommercialDocumentFormScreen
      {...form}
      title={form.isEditing ? 'Editar Orçamento' : 'Novo Orçamento'}
      breadcrumbEntityLabel="Orçamentos"
      breadcrumbCurrentLabel={form.isEditing ? 'Editar' : 'Novo Orçamento'}
      observationsPlaceholder="Observações do orçamento..."
      onGoBack={onGoBack}
    />
  );
}
