using Application.Shared.Queries.Interfaces;
using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;

namespace Application.Territories.Localities.Queries.Interfaces
{
    public interface ILocalityQuery : IEntityCrudQuery<LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
    }
}
