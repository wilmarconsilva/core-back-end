import { formatWithMask, Masks } from 'react-native-mask-input';

/** Formata um valor numérico para exibição com máscara monetária brasileira (R$). */
export function formatCurrencyInput(value: number): string {
  if (value === 0)
    return '';

  const valueToMask = Math.round(Math.max(0, value) * 100);

  return formatWithMask({ text: String(valueToMask), mask: Masks.BRL_CURRENCY }).masked;
}

/** Converte o texto mascarado (R$ 1.234,56) em valor numérico. */
export function parseCurrencyInput(value: string): number {
  const digits = value.replace(/\D/g, '');

  if (!digits) return 0;

  return parseInt(digits, 10) / 100;
}
