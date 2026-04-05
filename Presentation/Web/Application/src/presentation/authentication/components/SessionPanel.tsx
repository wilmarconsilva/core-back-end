import React from 'react';
import { Text, View } from 'react-native';
import { styles } from '../styles';

type SessionPanelProps = {
  accessTokenPreview: string;
  formattedExpiration: string;
  isAuthenticated: boolean;
};

export function SessionPanel({ accessTokenPreview, formattedExpiration, isAuthenticated }: SessionPanelProps) {
  return (
    <View style={styles.sessionPanel}>
      <Text style={styles.sessionTitle}>Sessão atual</Text>
      <SessionRow label="Autenticado" value={isAuthenticated ? 'Sim' : 'Não'} />
      <SessionRow label="Expiração do access token" value={formattedExpiration} />
      <SessionRow label="Access token" value={accessTokenPreview} />
    </View>
  );
}

type SessionRowProps = {
  label: string;
  value: string;
};

function SessionRow({ label, value }: SessionRowProps) {
  return (
    <View style={styles.sessionRow}>
      <Text style={styles.sessionLabel}>{label}</Text>
      <Text style={styles.sessionValue}>{value}</Text>
    </View>
  );
}
