using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Application.Interfaces.Queries;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public abstract class CrudService<TEntity, TEntitySaveDto> : ICrudService<TEntitySaveDto> where TEntity : Entity where TEntitySaveDto : EntitySaveDto
    {
        #region Attributes

        protected readonly IRepositoryCrud<TEntity> _repositoryCrud;
        protected readonly IEntitySaveDtoQuery<TEntitySaveDto> _query;

        private readonly IUnitOfWork _unitOfWork;

        private readonly bool _hasExternalTransactionActive;

        #endregion Attributes

        #region Constructors

        protected CrudService(IRepositoryCrud<TEntity> repository, IEntitySaveDtoQuery<TEntitySaveDto> query, IUnitOfWork unitOfWork)
        {
            this._repositoryCrud = repository;
            this._query = query;
            this._unitOfWork = unitOfWork;
            this._hasExternalTransactionActive = _unitOfWork.IsTransactionBegun();
        }

        #endregion Constructors

        #region Public methods

        public Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken)
        {
            ValidateDtos(dtos);

            return SaveAsync(dtos, cancellationToken);
        }

        public async Task<TEntitySaveDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            TEntitySaveDto? entitySaveDto = await _query.GetSaveDto(id, cancellationToken);

            if (entitySaveDto == null)
                ThrowNotFoundException(id);

            return entitySaveDto!;
        }

        public async Task UpdateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken)
        {
            ValidateDtos(dtos);

            IEnumerable<long> dtosIds = dtos.Select(d => d.Id);

            await SaveAsync(dtos, cancellationToken);
        }

        public Task UpdateAsync(long id, TEntitySaveDto dto, CancellationToken cancellationToken)
        {
            dto.Id = id;
            return UpdateAsync([dto], cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            await ValidateIds(ids, cancellationToken);

            await EntitiesCanBeDeleted(ids);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _repositoryCrud.DeleteAsync(ids, cancellationToken);

            await CommitAsync(cancellationToken);
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            await DeleteAsync([id], cancellationToken);
        }

        public abstract Task<List<TListing>> GetAllAsync<TListing, TFilter>(TFilter filter, CancellationToken cancellationToken)
            where TListing : EntityListing
            where TFilter : EntityListingFilter;

        #endregion Public methods

        #region Protected methods

        protected abstract TEntity CreateEntity(TEntitySaveDto dto);

        protected abstract void UpdateEntity(TEntitySaveDto dto, TEntity entity);

        protected abstract Task<bool> EntitiesCanBeDeleted(IEnumerable<long> ids);

        protected static void ThrowNotFoundException(IEnumerable<long> ids)
        {
            throw new NotFoundException(GetNotFoundMessage(ids));
        }

        protected static string GetNotFoundMessage(IEnumerable<long> ids)
        {
            return $"Nenhum registro encontrado para o(s) código(s): {string.Join(",", ids)}.";
        }

        protected async Task<IEnumerable<Entity>> SaveAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken)
        {
            List<TEntity> entities = await TransformToEntity(dtos, cancellationToken);

            await this.SaveAsync(entities, cancellationToken);

            return entities;
        }

        protected async Task SaveAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _repositoryCrud.SaveAsync(entities, cancellationToken);

            await CommitAsync(cancellationToken);
        }

        #endregion Protected methods

        #region Private methods

        private Task CommitAsync(CancellationToken cancellationToken)
        {
            if (!_hasExternalTransactionActive)
                return _unitOfWork.CommitAsync(cancellationToken);

            return Task.CompletedTask;
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

        private async Task<List<TEntity>> TransformToEntity(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken)
        {
            IEnumerable<long> existingIds = dtos.Where(dto => dto.Id > 0).Select(dto => dto.Id);

            List<TEntity> entities;
            if (existingIds.Any())
                entities = await _repositoryCrud.GetByIdsAsync(existingIds, cancellationToken);
            else
                entities = new List<TEntity>(dtos.Count());

            foreach (TEntitySaveDto dto in dtos)
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

        #region Private static methods

        private static void ThrowNotFoundException(long id)
        {
            ThrowNotFoundException([id]);
        }

        private static void ValidateDtos(IEnumerable<TEntitySaveDto> dtos)
        {
            if (dtos == null || !dtos.Any())
                throw new BusinessRuleException("Nenhuma informação foi fornecida para executar a operação.");
        }

        #endregion Private static methods

        #endregion Private methods
    }
}