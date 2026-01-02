using Application.DataTransferObjects;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICrudService<TEntitySaveDto> where TEntitySaveDto : EntitySaveDto
    {
        Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken);

        Task<Entity> CreateAsync(TEntitySaveDto dto, CancellationToken cancellationToken);

        Task<TEntitySaveDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task UpdateAsync(long id, TEntitySaveDto dto, CancellationToken cancellationToken);

        Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);

        Task<IEnumerable<Entity>> SaveAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken);
    }
}