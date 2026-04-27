import { CrudService } from './CrudService';
import { IProductService } from '../Interfaces/Services/IProductService';
import { IProductSaveDto } from '../Interfaces/DataTransferObjects/Products/IProductSaveDto';
import { IProductListing } from '../Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../Interfaces/DataTransferObjects/Products/IProductListingFilter';

export class ProductService
  extends CrudService<IProductSaveDto, IProductListing, IProductListingFilter>
  implements IProductService
{
  constructor() {
    super('/products');
  }
}
