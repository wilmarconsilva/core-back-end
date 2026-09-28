using Application.Shared.DataTransferObjects.Entities;

namespace Application.Territories.Localities.DataTransferObjects.Listings
{
    public class LocalityListingFilter : EntityListingFilter
    {
        public LocalityListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public LocalityListingFilter() : base() { }

        public string? Description { get; init; }

        public long? StateId { get; init; }
    }
}
