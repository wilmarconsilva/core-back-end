using Domain.Products.Entities;
using Domain.Products.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Repositories.Persons.Customers
{
    public class ProductRepository(AppDbContext context) : RepositoryCrud<Product>(context), IProductRepository
    {

    }
}