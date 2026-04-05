import { StatusBar } from 'expo-status-bar';
import {
  ActivityIndicator,
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  Text,
  TextInput,
  View,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { ActionButton } from '../components/ActionButton';
import { SessionPanel } from '../components/SessionPanel';
import { styles } from '../styles';
import { AuthenticationViewModel } from '../viewModels/AuthenticationViewModel';

type LoginScreenProps = {
  viewModel: AuthenticationViewModel;
};

export function LoginScreen({ viewModel }: LoginScreenProps) {
  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />
      <KeyboardAvoidingView
        style={styles.keyboardContainer}
        behavior={Platform.OS === 'ios' ? 'padding' : undefined}
      >
        <ScrollView contentContainerStyle={styles.scrollContent}>
          <View style={styles.card}>
            <Text style={styles.title}>Core Authentication</Text>
            <Text style={styles.subtitle}>
              O refresh token é mantido em cookie HttpOnly. O front trabalha apenas com o access token.
            </Text>

            <View style={styles.fieldGroup}>
              <Text style={styles.label}>URL da API</Text>
              <TextInput
                autoCapitalize="none"
                autoCorrect={false}
                editable={!viewModel.isSubmitting}
                onChangeText={viewModel.setApiBaseUrl}
                placeholder="https://localhost:7294"
                placeholderTextColor="#94a3b8"
                style={styles.input}
                value={viewModel.apiBaseUrl}
              />
              <Text style={styles.helper}>Padrão do profile HTTPS: `https://localhost:7294`.</Text>
            </View>

            <View style={styles.fieldGroup}>
              <Text style={styles.label}>E-mail</Text>
              <TextInput
                autoCapitalize="none"
                autoCorrect={false}
                editable={!viewModel.isSubmitting}
                keyboardType="email-address"
                onChangeText={viewModel.setEmail}
                placeholder="usuario@exemplo.com"
                placeholderTextColor="#94a3b8"
                style={styles.input}
                textContentType="emailAddress"
                value={viewModel.email}
              />
            </View>

            <View style={styles.fieldGroup}>
              <Text style={styles.label}>Senha</Text>
              <TextInput
                autoCapitalize="none"
                autoCorrect={false}
                editable={!viewModel.isSubmitting}
                onChangeText={viewModel.setPassword}
                placeholder="Sua senha"
                placeholderTextColor="#94a3b8"
                secureTextEntry
                style={styles.input}
                textContentType="password"
                value={viewModel.password}
              />
            </View>

            <View style={styles.actions}>
              <ActionButton
                disabled={viewModel.isSubmitting}
                label="Entrar"
                onPress={() => {
                  void viewModel.login();
                }}
                primary
              />
              <ActionButton
                disabled={viewModel.isSubmitting || (!viewModel.isAuthenticated && !viewModel.isRestoringSession)}
                label="Renovar sessão"
                onPress={() => {
                  void viewModel.refreshSession();
                }}
              />
              <ActionButton
                disabled={viewModel.isSubmitting || !viewModel.isAuthenticated}
                label="Sair"
                onPress={() => {
                  void viewModel.logout();
                }}
              />
            </View>

            {(viewModel.isSubmitting || viewModel.isRestoringSession) && (
              <View style={styles.loadingContainer}>
                <ActivityIndicator color="#2563eb" />
                <Text style={styles.loadingText}>
                  {viewModel.isRestoringSession ? 'Restaurando sessão...' : 'Processando...'}
                </Text>
              </View>
            )}

            <View style={styles.feedbackPanel}>
              <Text style={styles.feedbackTitle}>Status</Text>
              <Text style={styles.feedbackText}>{viewModel.feedback}</Text>
            </View>

            <SessionPanel
              accessTokenPreview={viewModel.accessTokenPreview}
              formattedExpiration={viewModel.formattedExpiration}
              isAuthenticated={viewModel.isAuthenticated}
            />
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}
