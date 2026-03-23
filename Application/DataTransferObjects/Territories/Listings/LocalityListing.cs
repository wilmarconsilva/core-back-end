using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Territories.Listings
{
    public class LocalityListing : EntityListing
    {
        public required string Description { get; init; }

        public long? StateId { get; init; }
    }
}
