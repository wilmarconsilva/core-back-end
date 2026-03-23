using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Products.Listings
{
    public class ProductListing : EntityListing
    {
        public required string Description { get; init; }
    }
}
