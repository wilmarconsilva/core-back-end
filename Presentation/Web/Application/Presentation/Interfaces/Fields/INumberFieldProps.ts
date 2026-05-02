import { TextInputProps } from 'react-native';

export interface INumberFieldProps extends Omit<TextInputProps, 'keyboardType'> {
  label: string;
  error?: string;
}
