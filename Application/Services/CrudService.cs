using Application.DataTransferObjects;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using AutoMapper;

namespace Application.Services
{
    public abstract class CrudService<TEntity, TEntitySaveDto> : ICrudService<TEntitySaveDto> where TEntity : Entity where TEntitySaveDto : EntitySaveDto
    {
        #region Attributes

        protected readonly IRepositoryCrud<TEntity> _repositoryCrud;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly bool _hasExternalTransactionActive;

        #endregion Attributes

        #region Constructors

        protected CrudService(IRepositoryCrud<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._repositoryCrud = repository;
            this._unitOfWork = unitOfWork;
            this._hasExternalTransactionActive = _unitOfWork.IsTransactionBegun();
            this._mapper = mapper;
        }

        #endregion Constructors

        #region Public methods

        public Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken)
        {
            ValidateDtos(dtos);

            return SaveAsync(dtos, cancellationToken);
        }

        public async Task<Entity> CreateAsync(TEntitySaveDto dto, CancellationToken cancellationToken)
        {
            return (await CreateAsync([dto], cancellationToken)).First();
        }

        public async Task<TEntitySaveDto> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            Entity? entity = await _repositoryCrud.GetByIdAsync(id, cancellationToken);

            if (entity == null)
                ThrowNotFoundException(id);

            return TransformToDto(entity!);
        }

        public async Task UpdateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken)
        {
            ValidateDtos(dtos);

            IEnumerable<long> dtosIds = dtos.Select(d => d.Id);

            await ValidateIds(dtosIds, cancellationToken);

            await SaveAsync(dtos, cancellationToken);
        }

        public Task UpdateAsync(long id, TEntitySaveDto dto, CancellationToken cancellationToken)
        {
            dto.Id = id;
            return UpdateAsync([dto], cancellationToken);
        }

        public async Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await ValidateIds(ids, cancellationToken);

            await _repositoryCrud.DeleteAsync(ids, cancellationToken);

            await CommitAsync(cancellationToken);
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            await DeleteAsync([id], cancellationToken);
        }

        #endregion Public methods

        #region Protected methods

        protected async virtual Task ValidateBusinessRulesForSaveAsync(EntityDto<TEntity, TEntitySaveDto>[] entitiesDtos, CancellationToken cancellationToken)
        {
            for (int i = 0; i < entitiesDtos.Length; i++)
            {
                string? message = await ValidateBusinessRulesForSaveAsync(entitiesDtos[i], cancellationToken);

                if (!string.IsNullOrWhiteSpace(message))
                    throw new BusinessRuleException(message);
            }
        }

        protected virtual Task<string?> ValidateBusinessRulesForSaveAsync(EntityDto<TEntity, TEntitySaveDto> entityDto, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>(null);
        }
        protected EntityDto<TEntity, TEntitySaveDto>[] TransformToEntity(IEnumerable<TEntitySaveDto> dtos)
        {
            return dtos.Select(TransformToEntity).ToArray();
        }

        protected virtual async Task SaveRelatedEntitiesAsync(EntityDto<TEntity, TEntitySaveDto>[] entitiesDtos, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        protected Task<IEnumerable<Entity>> SaveAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken)
        {
            EntityDto<TEntity, TEntitySaveDto>[] entitiesDtos = TransformToEntity(dtos);

            return this.SaveAsync(entitiesDtos, cancellationToken);
        }

        #endregion Protected methods

        #region Private methods

        private async Task<IEnumerable<Entity>> SaveAsync(EntityDto<TEntity, TEntitySaveDto>[] entitiesDtos, CancellationToken cancellationToken)
        {
            IEnumerable<TEntity> entities = entitiesDtos.Select(ed => ed.Entity);

            await ValidateBusinessRulesForSaveAsync(entitiesDtos, cancellationToken);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _repositoryCrud.UpdateAsync(entities.Where(e => e.Id > 0), cancellationToken);

            await _repositoryCrud.AddAsync(entities.Where(e => e.Id == 0), cancellationToken);

            await _unitOfWork.FlushAsync(cancellationToken);

            await SaveRelatedEntitiesAsync(entitiesDtos, cancellationToken);

            await CommitAsync(cancellationToken);

            return entities;
        }

        private EntityDto<TEntity, TEntitySaveDto> TransformToEntity(TEntitySaveDto dto)
        {
            TEntity entity = _mapper.Map<TEntity>(dto);

            return new EntityDto<TEntity, TEntitySaveDto>
            {
                Entity = entity,
                Dto = dto
            };
        }

        private TEntitySaveDto TransformToDto(Entity entity)
        {
            return _mapper.Map<TEntitySaveDto>(entity);
        }

        private Task CommitAsync(CancellationToken cancellationToken)
        {
            if (!_hasExternalTransactionActive)
                return _unitOfWork.CommitAsync(cancellationToken);

            return Task.CompletedTask;
        }

        private async Task<IEnumerable<long>> GetNotExistingIds(IEnumerable<long> dtosIds, CancellationToken cancellationToken)
        {
            List<long> existingIds = await _repositoryCrud.GetIdsAsync(dtosIds, cancellationToken);

            return dtosIds.Except(existingIds);
        }

        private async Task ValidateIds(IEnumerable<long> dtosIds, CancellationToken cancellationToken)
        {
            IEnumerable<long> notExistingIds = await GetNotExistingIds(dtosIds, cancellationToken);

            if (notExistingIds.Any())
                ThrowNotFoundException(notExistingIds);
        }

        #region Private static methods

        private static void ThrowNotFoundException(IEnumerable<long> ids)
        {
            throw new NotFoundException($"Nenhum cadastro encontrado para o(s) código(s): {string.Join(",", ids)}.");
        }

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