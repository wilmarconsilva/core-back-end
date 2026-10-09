using Application.Shared.DataTransferObjects.Entities;
using Application.Shared.DataTransferObjects.Listings;
using Application.Shared.Queries.Interfaces;
using Application.Shared.Services.Interfaces;
using Domain.Shared.Exceptions;
using Domain.Shared.Entities;
using Domain.Shared.Persistance.Interfaces;
using Domain.Shared.Repositories.Interfaces;

namespace Application.Shared.Services
{
    public abstract class CrudService<TEntity, TSaveDto, TDetailDto, TListing, TFilter> : ICrudService<TSaveDto, TDetailDto, TListing, TFilter>
        where TEntity : Entity
        where TSaveDto : EntitySaveDto
        where TDetailDto : class
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        protected readonly IRepositoryCrud<TEntity> _repositoryCrud;
        protected readonly IEntityCrudQuery<TDetailDto, TListing, TFilter> _query;

        private readonly IUnitOfWork _unitOfWork;

        protected CrudService(IRepositoryCrud<TEntity> repository, IEntityCrudQuery<TDetailDto, TListing, TFilter> query, IUnitOfWork unitOfWork)
        {
            _repositoryCrud = repository;
            _query = query;
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TSaveDto> dtos, CancellationToken cancellationToken)
        {
            ValidateDtos(dtos);

            return SaveAsync(dtos, cancellationToken);
        }

        public async Task<TDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            TDetailDto? detail = await _query.GetDetailAsync(id, cancellationToken);

            if (detail == null)
                ThrowNotFoundException(id);

            return detail!;
        }

        public async Task UpdateAsync(IEnumerable<TSaveDto> dtos, CancellationToken cancellationToken)
        {
            ValidateDtos(dtos);

            await SaveAsync(dtos, cancellationToken);
        }

        public Task UpdateAsync(long id, TSaveDto dto, CancellationToken cancellationToken)
        {
            dto.Id = id;
            return UpdateAsync([dto], cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            await ValidateIds(ids, cancellationToken);

            if (!await EntitiesCanBeDeleted(ids))
                throw new BusinessRuleException("Os registros informados não podem ser excluídos.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _repositoryCrud.DeleteAsync(ids, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            await DeleteAsync([id], cancellationToken);
        }

        public Task<List<TListing>> GetAllAsync(TFilter filter, CancellationToken cancellationToken)
        {
            return _query.GetAll(filter, cancellationToken);
        }

        protected abstract TEntity CreateEntity(TSaveDto dto);

        protected abstract void UpdateEntity(TSaveDto dto, TEntity entity);

        protected abstract Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids);

        protected static void ThrowNotFoundException(IEnumerable<long> ids)
        {
            throw new NotFoundException(GetNotFoundMessage(ids));
        }

        protected static string GetNotFoundMessage(IEnumerable<long> ids)
        {
            return $"Nenhum registro encontrado para o(s) código(s): {string.Join(",", ids)}.";
        }

        protected async Task<IEnumerable<Entity>> SaveAsync(IEnumerable<TSaveDto> dtos, CancellationToken cancellationToken)
        {
            List<TEntity> entities = await TransformToEntity(dtos, cancellationToken);

            await SaveAsync(entities, cancellationToken);

            return entities;
        }

        protected virtual async Task SaveAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _repositoryCrud.SaveAsync(entities, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task<IEnumerable<long>> GetNotExistingIds(IEnumerable<long> dtosIds, CancellationToken cancellationToken)
        {
            IReadOnlyList<long> existingIds = await _repositoryCrud.GetIdsAsync(dtosIds, cancellationToken);

            return dtosIds.Except(existingIds);
        }

        private async Task ValidateIds(IEnumerable<long> dtosIds, CancellationToken cancellationToken)
        {
            IEnumerable<long> notExistingIds = await GetNotExistingIds(dtosIds, cancellationToken);

            if (notExistingIds.Any())
                ThrowNotFoundException(notExistingIds);
        }

        private async Task<List<TEntity>> TransformToEntity(IEnumerable<TSaveDto> dtos, CancellationToken cancellationToken)
        {
            IEnumerable<long> existingIds = dtos.Where(dto => dto.Id > 0).Select(dto => dto.Id);

            List<TEntity> entities;
            if (existingIds.Any())
                entities = await _repositoryCrud.GetByIdsAsync(existingIds, cancellationToken);
            else
                entities = new List<TEntity>(dtos.Count());

            foreach (TSaveDto dto in dtos)
            {
                TEntity? entity;

                if (dto.Id > 0)
                {
                    entity = entities.Find(c => c.Id == dto.Id);

                    if (entity == null)
                        ThrowNotFoundException(dto.Id);
                    else
                    {
                        UpdateEntity(dto, entity);
                    }
                }
                else
                {
                    entity = CreateEntity(dto);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        private static void ThrowNotFoundException(long id)
        {
            ThrowNotFoundException([id]);
        }

        private static void ValidateDtos(IEnumerable<TSaveDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                throw new BusinessRuleException("Nenhuma informação foi fornecida para executar a operação.");
        }
    }
}
