using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;

namespace Application.Interfaces.Services
{
    public interface IProductService : ICrudService<ProductSaveDto, ProductDetailDto, ProductListing, ProductListingFilter>
    {
    }
}
