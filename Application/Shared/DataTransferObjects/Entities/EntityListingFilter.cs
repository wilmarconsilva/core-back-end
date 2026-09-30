using Application.Shared.DataTransferObjects.Listings;

namespace Application.Shared.DataTransferObjects.Entities
{
    public abstract class EntityListingFilter : ListingFilter
    {
        protected EntityListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        protected EntityListingFilter() : base() { }

        public long? Id { get; init; }
    }
}