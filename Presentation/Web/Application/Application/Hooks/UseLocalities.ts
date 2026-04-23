import { useCallback, useMemo } from 'react';
import { LocalityService } from '../Services/LocalityService';
import { LocalitySaveDto } from '../Interfaces/DataTransferObjects/LocalitySaveDto';
import { LocalityListing } from '../Interfaces/DataTransferObjects/LocalityListing';
import { LocalityListingFilter } from '../Interfaces/DataTransferObjects/LocalityListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const localityService = new LocalityService();

export type UseLocalitiesResult = UseCrudResult<LocalitySaveDto, LocalityListing, LocalityListingFilter> & {
  loadAllForSelect: () => Promise<void>;
};

export function UseLocalities(): UseLocalitiesResult {
  const service = useMemo(() => localityService, []);
  const crud = UseCrud(service);
  const { loadAll } = crud;

  const loadAllForSelect = useCallback(async () => {
    await loadAll({ registersStart: 0, registersEnd: 200 });
  }, [loadAll]);

  return { ...crud, loadAllForSelect };
}
