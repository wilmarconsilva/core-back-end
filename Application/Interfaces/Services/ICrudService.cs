using Application.DataTransferObjects;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICrudService<TEntity, TEntitySaveDto> where TEntity : Entity where TEntitySaveDto : EntitySaveDto
    {
        Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken);

        Task<Entity> CreateAsync(TEntitySaveDto dto, CancellationToken cancellationToken);

        Task<TEntitySaveDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task UpdateAsync(long id, TEntitySaveDto dto, CancellationToken cancellationToken);

        Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);

        Task SaveByRelatedEntitiesAsync<TRelatedEntity, TRelatedEntitySaveDto>(EntityDto<TRelatedEntity, TRelatedEntitySaveDto>[] relatedEntitiesDtos, Func<TRelatedEntitySaveDto, IEnumerable<TEntitySaveDto>> getEntitySaveDtoFromRelated, Action<TEntity, TRelatedEntity> setRelation, CancellationToken cancellationToken) where TRelatedEntity : Entity where TRelatedEntitySaveDto : EntitySaveDto;
    }
}