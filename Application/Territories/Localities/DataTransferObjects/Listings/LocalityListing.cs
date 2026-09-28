using Application.Shared.DataTransferObjects.Entities;

namespace Application.Territories.Localities.DataTransferObjects.Listings
{
    public class LocalityListing : EntityListing
    {
        public required string Description { get; init; }

        public long? StateId { get; init; }
    }
}
