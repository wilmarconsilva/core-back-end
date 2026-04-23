import React from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../Components/Breadcrumb';
import { MenuItem } from '../Components/MenuItem';
import { gridStyles, layoutStyles, typographyStyles } from '../Styles';
import { UseNavigation, Screen } from '../../Application/Hooks/UseNavigation';
import { ReportsScreen } from './ReportsScreen';
import { CustomersListScreen } from './CustomersListScreen';
import { CustomerFormScreen } from './CustomerFormScreen';
import { ProductsListScreen } from './ProductsListScreen';
import { ProductFormScreen } from './ProductFormScreen';
import { LocalitiesListScreen } from './LocalitiesListScreen';
import { LocalityFormScreen } from './LocalityFormScreen';
import { BudgetsListScreen } from './BudgetsListScreen';
import { BudgetFormScreen } from './BudgetFormScreen';
import { OrdersListScreen } from './OrdersListScreen';
import { OrderFormScreen } from './OrderFormScreen';
import { BudgetReportScreen } from './BudgetReportScreen';
import { OrderReportScreen } from './OrderReportScreen';

interface HomeScreenProps {
  userName: string;
}

interface HomeMenuProps {
  userName: string;
  navigate: (screen: Screen) => void;
}

function HomeMenuScreen({ userName, navigate }: HomeMenuProps) {
  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb items={[{ label: 'Início' }]} />
        <Text style={typographyStyles.title}>Olá, {userName}!</Text>
        <Text style={typographyStyles.subtitle}>O que deseja acessar?</Text>
        <View style={gridStyles.grid}>
          <MenuItem icon="people-outline" label="Clientes" onPress={() => navigate({ name: 'customers' })} />
          <MenuItem icon="cube-outline" label="Produtos" onPress={() => navigate({ name: 'products' })} />
          <MenuItem icon="cash-outline" label="Orçamentos" onPress={() => navigate({ name: 'budgets' })} />
          <MenuItem icon="clipboard-outline" label="Pedidos" onPress={() => navigate({ name: 'orders' })} />
          <MenuItem icon="location-outline" label="Localidades" onPress={() => navigate({ name: 'localities' })} />
          <MenuItem icon="bar-chart-outline" label="Relatórios" onPress={() => navigate({ name: 'reports' })} />
        </View>
      </ScrollView>
    </View>
  );
}

export function HomeScreen({ userName }: HomeScreenProps) {
  const { currentScreen, navigate, goBack } = UseNavigation();

  switch (currentScreen.name) {
    case 'customers':
      return <CustomersListScreen onGoBack={goBack} onNavigateToForm={(id) => navigate({ name: 'customerForm', id })} />;
    case 'customerForm':
      return <CustomerFormScreen id={currentScreen.id} onGoBack={goBack} />;

    case 'products':
      return <ProductsListScreen onGoBack={goBack} onNavigateToForm={(id) => navigate({ name: 'productForm', id })} />;
    case 'productForm':
      return <ProductFormScreen id={currentScreen.id} onGoBack={goBack} />;

    case 'budgets':
      return <BudgetsListScreen onGoBack={goBack} onNavigateToForm={(id) => navigate({ name: 'budgetForm', id })} />;
    case 'budgetForm':
      return <BudgetFormScreen id={currentScreen.id} onGoBack={goBack} />;

    case 'orders':
      return <OrdersListScreen onGoBack={goBack} onNavigateToForm={(id) => navigate({ name: 'orderForm', id })} />;
    case 'orderForm':
      return <OrderFormScreen id={currentScreen.id} onGoBack={goBack} />;

    case 'localities':
      return <LocalitiesListScreen onGoBack={goBack} onNavigateToForm={(id) => navigate({ name: 'localityForm', id })} />;
    case 'localityForm':
      return <LocalityFormScreen id={currentScreen.id} onGoBack={goBack} />;

    case 'reports':
      return (
        <ReportsScreen
          onGoBack={goBack}
          onNavigateToBudgetReport={() => navigate({ name: 'budgetReport' })}
          onNavigateToOrderReport={() => navigate({ name: 'orderReport' })}
        />
      );
    case 'budgetReport':
      return <BudgetReportScreen onGoBack={goBack} />;
    case 'orderReport':
      return <OrderReportScreen onGoBack={goBack} />;

    default:
      return <HomeMenuScreen userName={userName} navigate={navigate} />;
  }
}

