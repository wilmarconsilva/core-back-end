using Application.DataTransferObjects.Products;
using Application.DataTransferObjects.Products.Listings;

namespace Application.Interfaces.Services
{
    public interface IProductService : ICrudService<ProductSaveDto, ProductDetailDto, ProductListing, ProductListingFilter>
    {
    }
}
