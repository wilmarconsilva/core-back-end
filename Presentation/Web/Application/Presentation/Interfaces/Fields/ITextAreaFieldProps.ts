import { TextInputProps } from 'react-native';

export interface ITextAreaFieldProps extends Omit<TextInputProps, 'multiline' | 'numberOfLines'> {
  label: string;
  error?: string;
  numberOfLines?: number;
}
