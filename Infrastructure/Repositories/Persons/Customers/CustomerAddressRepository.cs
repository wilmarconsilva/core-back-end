using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories.Persons.Customers
{
    public class CustomerAddressRepository(AppDbContext context) : RepositoryCrud<CustomerAddress>(context), ICustomerAddressRepository
    {
        
    }
}