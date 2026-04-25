import { Ionicons } from '@expo/vector-icons';
import React from 'react';
import { StyleSheet, Text, TouchableOpacity, View } from 'react-native';
import Toast, { ToastConfig, ToastConfigParams } from 'react-native-toast-message';
import { colors } from '../Styles';

type ToastVariant = 'success' | 'error' | 'info';

const variantConfig: Record<ToastVariant, { accent: string; icon: keyof typeof Ionicons.glyphMap; iconColor: string }> = {
  success: { accent: colors.primary, icon: 'checkmark-circle-outline', iconColor: colors.primary },
  error:   { accent: '#DC2626',      icon: 'alert-circle-outline',     iconColor: '#DC2626'      },
  info:    { accent: '#2563EB',      icon: 'information-circle-outline', iconColor: '#2563EB'    },
};

function ToastItem({ text1, text2, type = 'info' }: ToastConfigParams<unknown> & { type?: ToastVariant }) {
  const { accent, icon, iconColor } = variantConfig[type as ToastVariant] ?? variantConfig.info;

  return (
    <TouchableOpacity
      activeOpacity={0.85}
      onPress={() => Toast.hide()}
      style={[styles.container, { borderLeftColor: accent }]}
    >
      <Ionicons name={icon} size={20} color={iconColor} style={styles.icon} />
      <View style={styles.textContainer}>
        {!!text1 && <Text style={styles.text1}>{text1}</Text>}
        {!!text2 && <Text style={styles.text2}>{text2}</Text>}
      </View>
    </TouchableOpacity>
  );
}

const toastConfig: ToastConfig = {
  success: (props) => <ToastItem {...props} type="success" />,
  error:   (props) => <ToastItem {...props} type="error" />,
  info:    (props) => <ToastItem {...props} type="info" />,
};

export function AppToast() {
  return (
    <Toast
      position="bottom"
      bottomOffset={40}
      config={toastConfig}
      autoHide={false}
    />
  );
}

const styles = StyleSheet.create({
  container: {
    width: '90%',
    flexDirection: 'row',
    alignItems: 'flex-start',
    backgroundColor: '#FFFFFF',
    borderRadius: 12,
    borderLeftWidth: 4,
    paddingVertical: 14,
    paddingHorizontal: 16,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.10,
    shadowRadius: 12,
    elevation: 6,
  },
  icon: {
    marginTop: 1,
    marginRight: 12,
  },
  textContainer: {
    flex: 1,
    gap: 2,
  },
  text1: {
    fontSize: 14,
    fontWeight: '600',
    color: colors.textPrimary,
    lineHeight: 20,
  },
  text2: {
    fontSize: 13,
    color: colors.textSecondary,
    lineHeight: 18,
  },
});
