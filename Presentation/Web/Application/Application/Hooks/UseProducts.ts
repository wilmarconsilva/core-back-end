import { useMemo } from 'react';
import { ProductService } from '../Services/Products/ProductService';
import { IProductSaveDto } from '../Interfaces/DataTransferObjects/Products/IProductSaveDto';
import { IProductDetailDto } from '../Interfaces/DataTransferObjects/Products/IProductDetailDto';
import { IProductListing } from '../Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const productService = new ProductService();

export function UseProducts(): UseCrudResult<IProductSaveDto, IProductDetailDto, IProductListing, IProductListingFilter> {
  const service = useMemo(() => productService, []);
  return UseCrud(service);
}
