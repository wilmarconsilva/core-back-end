using Application.Shared.DataTransferObjects.Entities;

namespace Application.Products.DataTransferObjects.Listings
{
    public class ProductListing : EntityListing
    {
        public required string Description { get; init; }
    }
}
