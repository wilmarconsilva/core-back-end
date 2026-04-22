import React, { useState } from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../Components/Breadcrumb';
import { MenuItem } from '../Components/MenuItem';
import { gridStyles, layoutStyles, typographyStyles } from '../Styles';
import { ReportsScreen } from './ReportsScreen';

interface HomeScreenProps {
  userName: string;
}

export function HomeScreen({ userName }: HomeScreenProps) {
  const [currentScreen, setCurrentScreen] = useState<'home' | 'reports'>('home');

  if (currentScreen === 'reports') {
    return (
      <ReportsScreen onGoBack={() => setCurrentScreen('home')} />
    );
  }

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb items={[{ label: 'Início' }]} />

        <Text style={typographyStyles.title}>Olá, {userName}!</Text>
        <Text style={typographyStyles.subtitle}>O que deseja acessar?</Text>

        <View style={gridStyles.grid}>
          <MenuItem
            icon="people-outline"
            label="Clientes"
            onPress={() => console.log('Navegar → Clientes')}
          />
          <MenuItem
            icon="cube-outline"
            label="Produtos"
            onPress={() => console.log('Navegar → Produtos')}
          />
          <MenuItem
            icon="cash-outline"
            label="Orçamentos"
            onPress={() => console.log('Navegar → Orçamentos')}
          />
          <MenuItem
            icon="clipboard-outline"
            label="Pedidos"
            onPress={() => console.log('Navegar → Pedidos')}
          />
          <MenuItem
            icon="bar-chart-outline"
            label="Relatórios"
            onPress={() => setCurrentScreen('reports')}
          />
        </View>
      </ScrollView>
    </View>
  );
}
