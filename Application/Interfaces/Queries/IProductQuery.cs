using Application.DataTransferObjects.Products;
using Application.DataTransferObjects.Products.Listings;

namespace Application.Interfaces.Queries
{
    public interface IProductQuery : IEntitySaveDtoQuery<ProductSaveDto>, IEntityListingQuery<ProductListing, ProductListingFilter>
    {

    }
}