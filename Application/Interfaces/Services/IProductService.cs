using Application.DataTransferObjects.Products;

namespace Application.Interfaces.Services
{
    public interface IProductService : ICrudService<ProductSaveDto>
    {
        
    }
}