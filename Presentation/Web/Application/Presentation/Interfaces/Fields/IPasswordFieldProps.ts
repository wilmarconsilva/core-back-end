import { TextInputProps } from 'react-native';

export interface IPasswordFieldProps extends Omit<TextInputProps, 'secureTextEntry'> {
  label: string;
  error?: string;
}
