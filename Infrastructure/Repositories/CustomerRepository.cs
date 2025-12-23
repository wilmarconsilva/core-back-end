using Domain.Entities.Persons;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories
{
    public class CustomerRepository(ApplicationDbContext context) : RepositoryCrud<Customer>(context), ICustomerRepository
    {

    }
}