import { DocumentSaveDto } from './DocumentSaveDto';
import { BudgetProductSaveDto } from './BudgetProductSaveDto';

export interface BudgetSaveDto extends DocumentSaveDto<BudgetProductSaveDto> {}
