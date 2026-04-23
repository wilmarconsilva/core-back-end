import { ICrudService } from './ICrudService';
import { ProductSaveDto } from '../DataTransferObjects/ProductSaveDto';
import { ProductListing } from '../DataTransferObjects/ProductListing';
import { ProductListingFilter } from '../DataTransferObjects/ProductListingFilter';

export interface IProductService extends ICrudService<ProductSaveDto, ProductListing, ProductListingFilter> {}
