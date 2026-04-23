import { DocumentSaveDto } from './DocumentSaveDto';
import { OrderProductSaveDto } from './OrderProductSaveDto';

export interface OrderSaveDto extends DocumentSaveDto<OrderProductSaveDto> {}
