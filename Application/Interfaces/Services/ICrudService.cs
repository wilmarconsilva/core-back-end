using Application.DataTransferObjects.Entities;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICrudService<TEntitySaveDto> where TEntitySaveDto : EntitySaveDto
    {
        Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken);

        Task<TEntitySaveDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task UpdateAsync(long id, TEntitySaveDto dto, CancellationToken cancellationToken);

        Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);

        Task<List<TListing>> GetAllAsync<TListing, TFilter>(TFilter filter, CancellationToken cancellationToken)
            where TListing : EntityListing
            where TFilter : EntityListingFilter;
    }
}