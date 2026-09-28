using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;

namespace Application.Interfaces.Queries
{
    public interface IProductQuery : IEntityCrudQuery<ProductDetailDto, ProductListing, ProductListingFilter>
    {
    }
}
