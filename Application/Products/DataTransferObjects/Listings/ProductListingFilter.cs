using Application.Shared.DataTransferObjects.Entities;

namespace Application.Products.DataTransferObjects.Listings
{
    public class ProductListingFilter : EntityListingFilter
    {
        public ProductListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public ProductListingFilter() : base() { }

        public string? Description { get; init; }
    }
}
