import { Ionicons } from '@expo/vector-icons';
import React from 'react';
import { Text, TouchableOpacity } from 'react-native';
import { IMenuItemProps } from '../Interfaces/MenuItem/IMenuItemProps';
import { colors, gridStyles } from '../Styles';

export function MenuItem({ icon, label, onPress }: IMenuItemProps) {
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
