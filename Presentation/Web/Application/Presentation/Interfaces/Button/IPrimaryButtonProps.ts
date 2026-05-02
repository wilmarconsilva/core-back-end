import { TouchableOpacityProps } from 'react-native';

export interface IPrimaryButtonProps extends TouchableOpacityProps {
  title: string;
  isLoading?: boolean;
}
