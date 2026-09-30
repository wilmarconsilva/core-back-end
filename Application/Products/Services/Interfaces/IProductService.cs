using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;
using Application.Shared.Services.Interfaces;

namespace Application.Products.Services.Interfaces
{
    public interface IProductService : ICrudService<ProductSaveDto, ProductDetailDto, ProductListing, ProductListingFilter>
    {
    }
}
