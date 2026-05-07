using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICrudService<TSaveDto, TDetailDto, TListing, TFilter>
        where TSaveDto : EntitySaveDto
        where TDetailDto : class
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        Task<IEnumerable<Entity>> CreateAsync(IEnumerable<TSaveDto> dtos, CancellationToken cancellationToken);

        Task<TDetailDto> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task UpdateAsync(long id, TSaveDto dto, CancellationToken cancellationToken);

        Task UpdateAsync(IEnumerable<TSaveDto> dtos, CancellationToken cancellationToken);

        Task DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);

        Task<List<TListing>> GetAllAsync(TFilter filter, CancellationToken cancellationToken);
    }
}
