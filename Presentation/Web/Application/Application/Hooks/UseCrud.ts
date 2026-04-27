import { useCallback, useState } from 'react';
import Toast from 'react-native-toast-message';
import { ICrudService } from '../Interfaces/Services/ICrudService';

interface CrudState<TSaveDto, TListing> {
  listings: TListing[];
  isLoadingList: boolean;
  currentItem: TSaveDto | null;
  isLoadingItem: boolean;
  isSaving: boolean;
  isDeleting: boolean;
}

interface CrudActions<TSaveDto, TFilter> {
  loadAll: (filter: TFilter) => Promise<void>;
  loadById: (id: number) => Promise<void>;
  save: (id: number | null, dto: TSaveDto) => Promise<boolean>;
  remove: (id: number) => Promise<boolean>;
  clearCurrentItem: () => void;
}

export type UseCrudResult<TSaveDto, TListing, TFilter> = CrudState<TSaveDto, TListing> & CrudActions<TSaveDto, TFilter>;

export function UseCrud<TSaveDto, TListing, TFilter>(
  service: ICrudService<TSaveDto, TListing, TFilter>,
): UseCrudResult<TSaveDto, TListing, TFilter> {
  const [listings, setListings] = useState<TListing[]>([]);
  const [isLoadingList, setIsLoadingList] = useState(false);
  const [currentItem, setCurrentItem] = useState<TSaveDto | null>(null);
  const [isLoadingItem, setIsLoadingItem] = useState(false);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

  const loadAll = useCallback(async (filter: TFilter) => {
    setIsLoadingList(true);
    try {
      const data = await service.getAll(filter);
      setListings(data);
    } catch {
      // Error already shown by APIConsumerService
    } finally {
      setIsLoadingList(false);
    }
  }, [service]);

  const loadById = useCallback(async (id: number) => {
    setIsLoadingItem(true);
    try {
      const data = await service.getById(id);
      setCurrentItem(data);
    } catch {
      // Error already shown by APIConsumerService
    } finally {
      setIsLoadingItem(false);
    }
  }, [service]);

  const save = useCallback(async (id: number | null, dto: TSaveDto): Promise<boolean> => {
    setIsSaving(true);
    try {
      if (id) {
        await service.update(id, dto);
      } else {
        await service.create(dto);
      }
      return true;
    } catch {
      return false;
    } finally {
      setIsSaving(false);
    }
  }, [service]);

  const remove = useCallback(async (id: number): Promise<boolean> => {
    setIsDeleting(true);
    try {
      await service.delete(id);
      return true;
    } catch {
      return false;
    } finally {
      setIsDeleting(false);
    }
  }, [service]);

  const clearCurrentItem = useCallback(() => {
    setCurrentItem(null);
  }, []);

  return {
    listings,
    isLoadingList,
    currentItem,
    isLoadingItem,
    isSaving,
    isDeleting,
    loadAll,
    loadById,
    save,
    remove,
    clearCurrentItem,
  };
}
