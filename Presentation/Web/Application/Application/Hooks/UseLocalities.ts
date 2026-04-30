import { useMemo } from 'react';
import { LocalityService } from '../Services/LocalityService';
import { ILocalitySaveDto } from '../Interfaces/DataTransferObjects/Localities/ILocalitySaveDto';
import { ILocalityListing } from '../Interfaces/DataTransferObjects/Localities/ILocalityListing';
import { ILocalityListingFilter } from '../Interfaces/DataTransferObjects/Localities/ILocalityListingFilter';
import { UseCrud, UseCrudResult } from './UseCrud';

const localityService = new LocalityService();

export function UseLocalities(): UseCrudResult<ILocalitySaveDto, ILocalityListing, ILocalityListingFilter> {
  const service = useMemo(() => localityService, []);
  return UseCrud(service);
}
