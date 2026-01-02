using Application.DataTransferObjects;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IRelatedEntityCrudService<TRelatedEntity, TRelatedEntitySaveDto> where TRelatedEntity : Entity where TRelatedEntitySaveDto : EntitySaveDto
    {
        Task SaveByRelatedEntitiesAsync(EntityDto<TRelatedEntity, TRelatedEntitySaveDto>[] relatedEntitiesDtos, CancellationToken cancellationToken);
    }
}