using Application.DataTransferObjects.Territories;
using Application.DataTransferObjects.Territories.Listings;

namespace Application.Interfaces.Services.Territories
{
    public interface ILocalityService : ICrudService<LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
        Task<long> GetQuantityAsync(CancellationToken cancellationToken);
    }
}