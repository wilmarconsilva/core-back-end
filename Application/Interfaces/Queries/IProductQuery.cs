using Application.DataTransferObjects.Products;
using Application.DataTransferObjects.Products.Listings;

namespace Application.Interfaces.Queries
{
    public interface IProductQuery : IEntityCrudQuery<ProductDetailDto, ProductListing, ProductListingFilter>
    {
    }
}
