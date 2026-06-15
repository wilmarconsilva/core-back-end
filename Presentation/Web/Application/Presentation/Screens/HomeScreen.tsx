import React from 'react';
import { ScrollView, Text, View } from 'react-native';
import { Breadcrumb } from '../Components/Breadcrumb';
import { MenuItem } from '../Components/MenuItem';
import { ScreenTransition } from '../Components/ScreenTransition';
import { DataSynchronizationIndicator } from '../Components/DataSynchronizationIndicator';
import { gridStyles, layoutStyles, typographyStyles } from '../Styles';
import { UseNavigation, Screen } from '../../Application/Hooks/UseNavigation';
import { UseOperationMode } from '../../Application/Hooks/UseOperationMode';
import { IHomeMenuProps } from '../Interfaces/Home/IHomeMenuProps';
import { IHomeScreenProps } from '../Interfaces/Home/IHomeScreenProps';
import { ReportsScreen } from './ReportsScreen';
import { CustomersListScreen } from './Customers/CustomersListScreen';
import { CustomerFormScreen } from './Customers/CustomerFormScreen';
import { ProductsListScreen } from './Products/ProductsListScreen';
import { ProductFormScreen } from './Products/ProductFormScreen';
import { LocalitiesListScreen } from './Localities/LocalitiesListScreen';
import { LocalityFormScreen } from './Localities/LocalityFormScreen';
import { BudgetsListScreen } from './Commercial/Documents/Budgets/BudgetsListScreen';
import { BudgetFormScreen } from './Commercial/Documents/Budgets/BudgetFormScreen';
import { OrdersListScreen } from './Commercial/Documents/Orders/OrdersListScreen';
import { OrderFormScreen } from './Commercial/Documents/Orders/OrderFormScreen';
import { BudgetReportScreen } from './Commercial/Documents/Budgets/BudgetReportScreen';
import { OrderReportScreen } from './Commercial/Documents/Orders/OrderReportScreen';

function HomeMenuScreen({ userName, navigate, isOffline }: IHomeMenuProps) {
  const budgetTarget: Screen = { name: 'budgets' };
  const orderTarget: Screen = { name: 'orders' };

  return (
    <View style={layoutStyles.screenWrapper}>
      <ScrollView contentContainerStyle={layoutStyles.scrollContainer}>
        <Breadcrumb items={[{ label: 'Início' }]} />
        <Text style={typographyStyles.title}>Olá, {userName}!</Text>
        <Text style={typographyStyles.subtitle}>O que deseja acessar?</Text>
        <View style={gridStyles.grid}>
          <MenuItem icon="people-outline" label="Clientes" onPress={() => navigate({ name: 'customers' })} />
          <MenuItem icon="cube-outline" label="Produtos" onPress={() => navigate({ name: 'products' })} />
          <MenuItem icon="cash-outline" label="Orçamentos" onPress={() => navigate(budgetTarget)} />
          <MenuItem icon="clipboard-outline" label="Pedidos" onPress={() => navigate(orderTarget)} />
          {!isOffline && (
            <>
              <MenuItem icon="location-outline" label="Localidades" onPress={() => navigate({ name: 'localities' })} />
              <MenuItem icon="bar-chart-outline" label="Relatórios" onPress={() => navigate({ name: 'reports' })} />
            </>
          )}
        </View>
      </ScrollView>
    </View>
  );
}

function screenKey(screen: Screen): string {
  return 'id' in screen ? `${screen.name}-${screen.id ?? 'new'}` : screen.name;
}

export function HomeScreen({ userName }: IHomeScreenProps) {
  const { currentScreen, navigate, goBack } = UseNavigation();
  const { isOffline } = UseOperationMode();

  function renderScreen() {
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
        return <HomeMenuScreen userName={userName} navigate={navigate} isOffline={isOffline} />;
    }
  }

  return (
    <View style={{ flex: 1 }}>
      <ScreenTransition screenKey={screenKey(currentScreen)}>
        {renderScreen()}
      </ScreenTransition>
      <DataSynchronizationIndicator />
    </View>
  );
}

