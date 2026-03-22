using Application.DataTransferObjects.Listings;

namespace Application.DataTransferObjects.Entities
{
    public abstract class EntityListingFilter : ListingFilter
    {
        public required long Id { get; init; }
    }
}