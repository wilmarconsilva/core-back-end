using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories.Persons.Customers
{
    public class CustomerRepository(AppDbContext context) : RepositoryCrud<Customer>(context), ICustomerRepository
    {

    }
}