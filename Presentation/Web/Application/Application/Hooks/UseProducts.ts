import { useCallback, useMemo } from 'react';
import { ProductService } from '../Services/ProductService';
import { ProductSaveDto } from '../Interfaces/DataTransferObjects/ProductSaveDto';
import { ProductListing } from '../Interfaces/DataTransferObjects/ProductListing';
import { ProductListingFilter } from '../IListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const productService = new ProductService();

export type UseProductsResult = UseCrudResult<ProductSaveDto, ProductListing, ProductListingFilter> & {
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
