using Domain.Persons.Customers.Entities;
using Domain.Shared.Repositories.Interfaces;

namespace Domain.Persons.Customers.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryCrud<Customer>
    {

    }
}