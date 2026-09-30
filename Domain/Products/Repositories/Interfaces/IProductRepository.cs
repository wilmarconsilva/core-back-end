using Domain.Products.Entities;
using Domain.Shared.Repositories.Interfaces;

namespace Domain.Products.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryCrud<Product>
    {

    }
}