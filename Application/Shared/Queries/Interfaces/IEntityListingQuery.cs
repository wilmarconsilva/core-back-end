using Application.Shared.DataTransferObjects.Entities;
using Application.Shared.DataTransferObjects.Listings;

namespace Application.Shared.Queries.Interfaces
{
    public interface IEntityListingQuery<TListing, TFilter>
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        Task<List<TListing>> GetAll(TFilter filter, CancellationToken cancellationToken);
    }
}