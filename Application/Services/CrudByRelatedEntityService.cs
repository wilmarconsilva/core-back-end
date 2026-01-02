using Application.DataTransferObjects;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services
{
    public abstract class CrudByRelatedEntityService<TEntitySaveDto, TRelatedEntity, TRelatedEntitySaveDto> : IRelatedEntityCrudService<TRelatedEntity, TRelatedEntitySaveDto> where TEntitySaveDto : EntitySaveDto where TRelatedEntity : Entity where TRelatedEntitySaveDto : EntitySaveDto
    {
        #region Fields

        private readonly ICrudService<TEntitySaveDto> _crudService;

        #endregion Fields

        #region Constructors

        protected CrudByRelatedEntityService(ICrudService<TEntitySaveDto> crudService)
        {
            _crudService = crudService;
        }

        #endregion Constructors

        #region Public methods

        public async Task SaveByRelatedEntitiesAsync(EntityDto<TRelatedEntity, TRelatedEntitySaveDto>[] relatedEntitiesDtos, CancellationToken cancellationToken)
        {
            if (relatedEntitiesDtos == null || relatedEntitiesDtos.Length == 0)
                return;

            List<TEntitySaveDto> entitiesSaveDtos = GetEntitiesSaveDtoFromRelatedEntities(relatedEntitiesDtos);

            if (entitiesSaveDtos.Count == 0)
                return;

            await _crudService.SaveAsync(entitiesSaveDtos, cancellationToken);
        }

        #endregion Public methods

        #region Private methods

        private List<TEntitySaveDto> GetEntitiesSaveDtoFromRelatedEntities(EntityDto<TRelatedEntity, TRelatedEntitySaveDto>[] relatedEntitiesDtos)
        {
            List<TEntitySaveDto> dtos = new();

            foreach (EntityDto<TRelatedEntity, TRelatedEntitySaveDto> relatedEntityDto in relatedEntitiesDtos)
            {
                dtos.AddRange(GetEntitiesSaveDtosFromRelatedEntity(relatedEntityDto));
            }

            return dtos;
        }

        #endregion Private methods

        #region Protected methods

        protected abstract List<TEntitySaveDto> GetEntitiesSaveDtosFromRelatedEntity(EntityDto<TRelatedEntity, TRelatedEntitySaveDto> relatedEntityDto);

        #endregion Protected methods
    }
}