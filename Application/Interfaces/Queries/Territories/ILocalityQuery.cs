using Application.DataTransferObjects.Territories;
using Application.DataTransferObjects.Territories.Listings;

namespace Application.Interfaces.Queries.Territories
{
    public interface ILocalityQuery : IEntityCrudQuery<LocalityDetailDto, LocalityListing, LocalityListingFilter>
    {
    }
}
