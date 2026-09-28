using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;

namespace Application.Interfaces.Queries.Territories
{
    public interface ILocalityQuery : IEntityCrudQuery<LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
    }
}
