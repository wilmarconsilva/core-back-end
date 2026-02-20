using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories.Persons.Customers
{
    public class ProductRepository(AppDbContext context) : RepositoryCrud<Product>(context), IProductRepository
    {

    }
}