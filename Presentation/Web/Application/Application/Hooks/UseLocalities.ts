import { useCallback, useMemo } from 'react';
import { LocalityService } from '../Services/LocalityService';
import { ILocalitySaveDto } from '../Interfaces/DataTransferObjects/Localities/ILocalitySaveDto';
import { ILocalityListing } from '../Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const localityService = new LocalityService();

export type UseLocalitiesResult = UseCrudResult<ILocalitySaveDto, IILocalityListing, ILocalityListingFilter> & {
  loadAllForSelect: () => Promise<void>;
};

export function UseLocalities(): UseLocalitiesResult {
  const service = useMemo(() => localityService, []);
  const crud = UseCrud(service);
  const { loadAll } = crud;

  const loadAllForSelect = useCallback(async () => {
    await loadAll({ registersStart: 0, registersEnd: 50 });
  }, [loadAll]);

  return { ...crud, loadAllForSelect };
}
