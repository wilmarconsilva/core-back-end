import { ICrudService } from './ICrudService';
import { IProductSaveDto } from '../DataTransferObjects/Products/IProductSaveDto';
import { IProductListing } from '../DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../DataTransferObjects/Products/IProductListingFilter';

export interface IProductService extends ICrudService<IProductSaveDto, IProductListing, IProductListingFilter> {}
