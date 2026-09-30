using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;

namespace Application.Shared.Queries.Interfaces
{
    public interface IProductQuery : IEntityCrudQuery<ProductDetailDto, ProductListing, ProductListingFilter>
    {
    }
}
