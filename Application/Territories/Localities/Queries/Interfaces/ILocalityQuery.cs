using Application.Shared.Queries.Interfaces;
using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;
using Application.Territories.Localities.DataTransferObjects.Synchronizations;

namespace Application.Territories.Localities.Queries.Interfaces
{
    public interface ILocalityQuery : IEntityCrudQuery<LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
        Task<List<LocalitySynchronizationDto>> GetSynchronization(LocalityListingFilter filter, CancellationToken cancellationToken);
    }
}
