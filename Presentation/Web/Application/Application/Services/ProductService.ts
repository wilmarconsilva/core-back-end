import { CrudService } from './CrudService';
import { IProductService } from '../Interfaces/Services/IProductService';
import { ProductSaveDto } from '../Interfaces/DataTransferObjects/ProductSaveDto';
import { ProductListing } from '../Interfaces/DataTransferObjects/ProductListing';
import { ProductListingFilter } from '../IListingFilter';

export class ProductService
  extends CrudService<ProductSaveDto, ProductListing, ProductListingFilter>
  implements IProductService
{
  constructor() {
    super('/products');
  }
}
