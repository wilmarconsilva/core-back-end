import { TextInputProps } from 'react-native';

export interface IDateFieldProps extends Omit<TextInputProps, 'keyboardType'> {
  label: string;
  error?: string;
}
