import { StyleSheet } from 'react-native';

export const styles = StyleSheet.create({
  safeArea: {
    flex: 1,
    backgroundColor: '#e2e8f0',
  },
  keyboardContainer: {
    flex: 1,
  },
  scrollContent: {
    flexGrow: 1,
    justifyContent: 'center',
    padding: 24,
  },
  card: {
    alignSelf: 'center',
    width: '100%',
    maxWidth: 560,
    backgroundColor: '#ffffff',
    borderRadius: 18,
    padding: 24,
    gap: 16,
  },
  title: {
    fontSize: 28,
    fontWeight: '700',
    color: '#0f172a',
  },
  subtitle: {
    fontSize: 15,
    lineHeight: 22,
    color: '#475569',
  },
  fieldGroup: {
    gap: 8,
  },
  label: {
    fontSize: 14,
    fontWeight: '600',
    color: '#334155',
  },
  helper: {
    fontSize: 12,
    color: '#64748b',
  },
  input: {
    borderWidth: 1,
    borderColor: '#cbd5e1',
    borderRadius: 12,
    backgroundColor: '#f8fafc',
    color: '#0f172a',
    fontSize: 16,
    paddingHorizontal: 14,
    paddingVertical: 12,
  },
  actions: {
    gap: 10,
  },
  button: {
    borderRadius: 12,
    paddingHorizontal: 16,
    paddingVertical: 14,
    alignItems: 'center',
  },
  buttonPrimary: {
    backgroundColor: '#2563eb',
  },
  buttonSecondary: {
    backgroundColor: '#eff6ff',
    borderWidth: 1,
    borderColor: '#bfdbfe',
  },
  buttonDisabled: {
    opacity: 0.6,
  },
  buttonText: {
    fontSize: 15,
    fontWeight: '700',
  },
  buttonTextPrimary: {
    color: '#ffffff',
  },
  buttonTextSecondary: {
    color: '#1d4ed8',
  },
  loadingContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 10,
  },
  loadingText: {
    color: '#334155',
    fontSize: 14,
  },
  feedbackPanel: {
    backgroundColor: '#f8fafc',
    borderRadius: 12,
    padding: 16,
    gap: 6,
  },
  feedbackTitle: {
    fontSize: 14,
    fontWeight: '700',
    color: '#0f172a',
  },
  feedbackText: {
    fontSize: 14,
    lineHeight: 20,
    color: '#334155',
  },
  sessionPanel: {
    backgroundColor: '#0f172a',
    borderRadius: 12,
    padding: 16,
    gap: 12,
  },
  sessionTitle: {
    fontSize: 16,
    fontWeight: '700',
    color: '#f8fafc',
  },
  sessionRow: {
    gap: 4,
  },
  sessionLabel: {
    fontSize: 12,
    color: '#94a3b8',
    textTransform: 'uppercase',
  },
  sessionValue: {
    fontSize: 14,
    color: '#e2e8f0',
  },
});
