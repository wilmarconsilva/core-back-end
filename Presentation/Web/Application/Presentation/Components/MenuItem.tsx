import { Ionicons } from '@expo/vector-icons';
import React from 'react';
import { Text, TouchableOpacity } from 'react-native';
import { colors, gridStyles } from '../Styles';

interface MenuItemProps {
  icon: keyof typeof Ionicons.glyphMap;
  label: string;
  onPress: () => void;
}

export function MenuItem({ icon, label, onPress }: MenuItemProps) {
  return (
    <TouchableOpacity
      style={gridStyles.menuItem}
      onPress={onPress}
      activeOpacity={0.7}
    >
      <Ionicons
        name={icon}
        size={32}
        color={colors.primary}
        style={gridStyles.menuItemIcon}
      />
      <Text style={gridStyles.menuItemText}>{label}</Text>
    </TouchableOpacity>
  );
}
