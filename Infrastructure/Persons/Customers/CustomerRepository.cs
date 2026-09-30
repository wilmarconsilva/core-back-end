using Domain.Persons.Customers.Entities;
using Domain.Persons.Customers.Repositories.Interfaces;
using Infrastructure.Shared;
using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persons.Customers
{
    public class CustomerRepository(AppDbContext context) : RepositoryCrud<Customer>(context), ICustomerRepository
    {
        protected override IQueryable<Customer> ApplyIncludesForGetByIds(IQueryable<Customer> query)
        {
            return query.Include(c => c.Addresses);
        }
    }
}