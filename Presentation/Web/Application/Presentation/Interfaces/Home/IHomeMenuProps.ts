import { Screen } from '../../../Application/Hooks/UseNavigation';

export interface IHomeMenuProps {
  userName: string;
  navigate: (screen: Screen) => void;
}
