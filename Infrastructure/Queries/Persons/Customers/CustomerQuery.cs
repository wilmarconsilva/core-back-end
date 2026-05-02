using Application.DataTransferObjects.Persons.Customers;
using Application.DataTransferObjects.Persons.Customers.Listings;
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

        public async Task<List<CustomerListing>> GetAll(CustomerListingFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<Customer> query = _query;

            if (filter.Id.HasValue)
                query = query.Where(customer => customer.Id == filter.Id.Value);

            if (filter.Name != null)
                query = query.Where(customer => EF.Functions.ILike(customer.Name, filter.Name));

            return await ApplyPaging(query, filter)
                .Select(customer => new CustomerListing
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Document = customer.Document,
                })
                .ToListAsync(cancellationToken);
        }

        protected override IQueryable<Customer> ApplyIncludes(IQueryable<Customer> query)
        {
            return query.Include(c => c.Addresses);
        }
    }
}