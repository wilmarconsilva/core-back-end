using Application.Shared.Services.Interfaces;
using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;

namespace Application.Territories.Localities.Services.Interfaces
{
    public interface ILocalityService : ICrudService<LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
        Task<long> GetQuantityAsync(CancellationToken cancellationToken);
    }
}