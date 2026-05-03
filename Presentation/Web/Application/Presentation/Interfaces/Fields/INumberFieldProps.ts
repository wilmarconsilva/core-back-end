import { TextInputProps } from 'react-native';
import { ILabeledFieldBase } from './ILabeledFieldBase';

export interface INumberFieldProps extends Omit<TextInputProps, 'keyboardType'>, ILabeledFieldBase {}
