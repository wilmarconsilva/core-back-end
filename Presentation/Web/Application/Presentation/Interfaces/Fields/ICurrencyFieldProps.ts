import { TextInputProps } from 'react-native';
import { ILabeledFieldBase } from './ILabeledFieldBase';

export interface ICurrencyFieldProps extends Omit<TextInputProps, 'keyboardType'>, ILabeledFieldBase {}
