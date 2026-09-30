using Domain.Products.Entities;
using Domain.Products.Repositories.Interfaces;
using Infrastructure.Shared;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Products
{
    public class ProductRepository(AppDbContext context) : RepositoryCrud<Product>(context), IProductRepository
    {

    }
}