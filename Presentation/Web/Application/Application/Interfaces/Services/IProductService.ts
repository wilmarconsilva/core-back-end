import { ICrudService } from './ICrudService';
import { ProductSaveDto } from '../DataTransferObjects/ProductSaveDto';
import { ProductListing } from '../DataTransferObjects/ProductListing';
import { ProductListingFilter } from '../IListingFilter';

export interface IProductService extends ICrudService<ProductSaveDto, ProductListing, ProductListingFilter> {}
