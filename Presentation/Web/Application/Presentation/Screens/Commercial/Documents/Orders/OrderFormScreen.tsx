import React from 'react';
import { UseOrderForm } from '../../../../../Application/Hooks/UseOrderForm';
import { IEntityFormScreenProps } from '../../../../Interfaces/Screens/IEntityFormScreenProps';
import { CommercialDocumentFormScreen } from '../CommercialDocumentFormScreen';

export function OrderFormScreen({ id, onGoBack }: IEntityFormScreenProps) {
  const form = UseOrderForm(id, onGoBack);

  return (
    <CommercialDocumentFormScreen
      {...form}
      title={form.isEditing ? 'Editar Pedido' : 'Novo Pedido'}
      breadcrumbEntityLabel="Pedidos"
      breadcrumbCurrentLabel={form.isEditing ? 'Editar' : 'Novo Pedido'}
      observationsPlaceholder="Observações do pedido..."
      onGoBack={onGoBack}
    />
  );
}
