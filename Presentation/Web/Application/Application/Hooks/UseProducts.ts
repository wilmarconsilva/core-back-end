import { useCallback, useMemo } from 'react';
import { ProductService } from '../Services/ProductService';
import { IProductSaveDto } from '../Interfaces/DataTransferObjects/Products/IProductSaveDto';
import { IProductListing } from '../Interfaces/DataTransferObjects/Products/IProductListing';
import { IProductListingFilter } from '../Interfaces/DataTransferObjects/Products/IProductListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const productService = new ProductService();

export type UseProductsResult = UseCrudResult<IProductSaveDto, IProductListing, IProductListingFilter> & {
  loadAllForSelect: () => Promise<void>;
};

export function UseProducts(): UseProductsResult {
  const service = useMemo(() => productService, []);
  const crud = UseCrud(service);
  const { loadAll } = crud;

  const loadAllForSelect = useCallback(async () => {
    await loadAll({ registersStart: 0, registersEnd: 50 });
  }, [loadAll]);

  return { ...crud, loadAllForSelect };
}
