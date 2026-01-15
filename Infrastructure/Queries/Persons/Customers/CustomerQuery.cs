using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Queries.Persons.Customers;
using AutoMapper;
using Domain.Entities.Persons.Customers;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries.Persons.Customers
{
    public class CustomerQuery : SaveDtoQuery<Customer, CustomerSaveDto>, ICustomerQuery
    {
        public CustomerQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        protected override IQueryable<Customer> ApplyIncludes(IQueryable<Customer> query)
        {
            return query.Include("_addresses");
        }
    }
}