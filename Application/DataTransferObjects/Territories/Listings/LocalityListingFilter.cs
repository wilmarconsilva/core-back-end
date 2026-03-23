using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Territories.Listings
{
    public class LocalityListingFilter : EntityListingFilter
    {
        public LocalityListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public LocalityListingFilter() : base() { }

        public string? Description { get; init; }

        public long? StateId { get; init; }
    }
}
