import React from 'react';
import { Text, TouchableOpacity, View } from 'react-native';
import { breadcrumbStyles } from '../Styles';

export interface BreadcrumbItem {
  label: string;
  onPress?: () => void;
}

interface BreadcrumbProps {
  items: BreadcrumbItem[];
}

export function Breadcrumb({ items }: BreadcrumbProps) {
  return (
    <View style={breadcrumbStyles.container}>
      {items.map((item, index) => {
        const isLast = index === items.length - 1;

        return (
          <React.Fragment key={index}>
            {index > 0 && (
              <Text style={breadcrumbStyles.separator}>›</Text>
            )}
            {isLast ? (
              <Text style={breadcrumbStyles.itemActive}>{item.label}</Text>
            ) : (
              <TouchableOpacity onPress={item.onPress}>
                <Text style={breadcrumbStyles.item}>{item.label}</Text>
              </TouchableOpacity>
            )}
          </React.Fragment>
        );
      })}
    </View>
  );
}
