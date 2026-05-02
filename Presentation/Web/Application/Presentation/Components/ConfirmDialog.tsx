import React from 'react';
import {
  Modal,
  Text,
  TouchableOpacity,
  View,
} from 'react-native';
import { IConfirmDialogProps } from '../Interfaces/ConfirmDialog/IConfirmDialogProps';
import { modalStyles } from '../Styles';
import { PrimaryButton } from './PrimaryButton';
import { SecondaryButton } from './SecondaryButton';

export function ConfirmDialog({
  visible,
  title,
  message,
  onConfirm,
  onCancel,
  isLoading,
  confirmLabel = 'Confirmar',
  cancelLabel = 'Cancelar',
}: IConfirmDialogProps) {
  return (
    <Modal
      visible={visible}
      transparent
      animationType="fade"
      onRequestClose={onCancel}
    >
      <TouchableOpacity
        style={modalStyles.overlay}
        activeOpacity={1}
        onPress={!isLoading ? onCancel : undefined}
      >
        <TouchableOpacity activeOpacity={1} style={modalStyles.dialog}>
          <Text style={modalStyles.title}>{title}</Text>
          <Text style={modalStyles.message}>{message}</Text>
          <View style={modalStyles.actions}>
            <SecondaryButton
              title={cancelLabel}
              onPress={onCancel}
              disabled={isLoading}
              style={{ flex: 1, marginTop: 0 }}
            />
            <PrimaryButton
              title={confirmLabel}
              isLoading={isLoading}
              onPress={onConfirm}
              style={{ flex: 1, marginTop: 0 }}
            />
          </View>
        </TouchableOpacity>
      </TouchableOpacity>
    </Modal>
  );
}
