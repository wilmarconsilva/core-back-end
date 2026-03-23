using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICrudService<TEntitySaveDto, TListing, TFilter>
        where TEntitySaveDto : EntitySaveDto
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TEntitySaveDto> dtos, CancellationToken cancellationToken);

        Task<TEntitySaveDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task UpdateAsync(long id, TEntitySaveDto dto, CancellationToken cancellationToken);

        Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);

        Task<List<TListing>> GetAllAsync(TFilter filter, CancellationToken cancellationToken);
    }
}