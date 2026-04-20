import React from 'react';
import Toast, { BaseToast, ErrorToast, ToastConfig } from 'react-native-toast-message';
import { colors } from '../Styles';

const toastConfig: ToastConfig = {
  error: (props) => (
    <ErrorToast
      {...props}
      style={{
        borderLeftColor: '#DC2626',
        backgroundColor: '#DC2626',
        borderRadius: 10,
      }}
      text1Style={{
        fontSize: 15,
        fontWeight: '700',
        color: colors.textWhite,
      }}
      text2Style={{
        fontSize: 13,
        color: colors.textWhite,
      }}
    />
  ),
  success: (props) => (
    <BaseToast
      {...props}
      style={{
        borderLeftColor: colors.primary,
        backgroundColor: colors.primary,
        borderRadius: 10,
      }}
      text1Style={{
        fontSize: 15,
        fontWeight: '700',
        color: colors.textWhite,
      }}
      text2Style={{
        fontSize: 13,
        color: colors.textWhite,
      }}
    />
  ),
};

export function AppToast() {
  return (
    <Toast
      position="bottom"
      bottomOffset={40}
      config={toastConfig}
      onPress={() => Toast.hide()}
    />
  );
}
