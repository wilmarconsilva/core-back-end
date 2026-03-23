using Application.DataTransferObjects.Territories;
using Application.DataTransferObjects.Territories.Listings;
using Application.Interfaces.Queries;

namespace Application.Interfaces.Queries.Territories
{
    public interface ILocalityQuery : IEntitySaveDtoQuery<LocalitySaveDto>, IEntityListingQuery<LocalityListing, LocalityListingFilter>
    {

    }
}
