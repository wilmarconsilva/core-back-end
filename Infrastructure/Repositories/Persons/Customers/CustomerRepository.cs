using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Persons.Customers
{
    public class CustomerRepository(AppDbContext context) : RepositoryCrud<Customer>(context), ICustomerRepository
    {
        protected override IQueryable<Customer> ApplyIncludesForGetByIds(IQueryable<Customer> query)
        {
            return query.Include(c => c.Addresses);
        }
    }
}