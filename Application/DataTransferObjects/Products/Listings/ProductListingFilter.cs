using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Products.Listings
{
    public class ProductListingFilter : EntityListingFilter
    {
        public ProductListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public ProductListingFilter() : base() { }

        public string? Description { get; init; }
    }
}
