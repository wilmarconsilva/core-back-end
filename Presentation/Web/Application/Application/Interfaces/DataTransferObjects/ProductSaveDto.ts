import { MeasureUnit } from './MeasureUnit';

export interface ProductSaveDto {
  id: number;
  description: string;
  measureUnit: MeasureUnit;
}
