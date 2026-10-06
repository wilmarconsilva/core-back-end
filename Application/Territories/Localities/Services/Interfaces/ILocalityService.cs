using Application.Shared.Services.Interfaces;
using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;
using Application.Territories.Localities.DataTransferObjects.Synchronizations;

namespace Application.Territories.Localities.Services.Interfaces
{
    public interface ILocalityService : ICrudService<LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
        Task<long> GetQuantityAsync(CancellationToken cancellationToken);

        Task<List<LocalitySynchronizationDto>> GetSynchronizationAsync(LocalityListingFilter filter, CancellationToken cancellationToken);
    }
}