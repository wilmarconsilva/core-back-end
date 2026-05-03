export interface ICrudService<TSaveDto, TListing, TFilter> {
  getAll(filter: TFilter): Promise<TListing[]>;
  getById(id: number): Promise<TSaveDto>;
  create(dto: TSaveDto): Promise<TSaveDto>;
  update(id: number, dto: TSaveDto): Promise<TSaveDto>;
  delete(id: number): Promise<void>;
}
