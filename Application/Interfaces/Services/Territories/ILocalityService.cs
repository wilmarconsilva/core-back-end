using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;

namespace Application.Interfaces.Services.Territories
{
    public interface ILocalityService : ICrudService<LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
        Task<long> GetQuantityAsync(CancellationToken cancellationToken);
    }
}