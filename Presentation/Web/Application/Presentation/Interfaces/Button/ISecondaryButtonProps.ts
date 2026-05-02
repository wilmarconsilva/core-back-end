import { TouchableOpacityProps } from 'react-native';

export interface ISecondaryButtonProps extends TouchableOpacityProps {
  title: string;
  isLoading?: boolean;
}
