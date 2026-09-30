using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.Persons.Customers.Entities;
using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Infrastructure.Shared.Persistance;
using Infrastructure.Shared;
using Infrastructure.Shared.Queries;
using Application.Persons.Customers.Queries;

namespace Infrastructure.Persons.Customers.Queries
{
    public class CustomerQuery : DetailDtoQuery<Customer, CustomerDetailDto>, ICustomerQuery
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
                query = query.WhereContains(customer => customer.Name, filter.Name);

            return await ApplyPaging(query, filter)
                .Select(customer => new CustomerListing
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    LocalityState = customer.Addresses
                            .OrderBy(address => address.Id)
                            .Select(address => address.Locality!.Description + (address.Locality.State == null ? string.Empty : $" - {address.Locality.State.Acronym}"))
                            .FirstOrDefault() ?? string.Empty,
                })
                .ToListAsync(cancellationToken);
        }

        protected override IQueryable<Customer> ApplyIncludes(IQueryable<Customer> query)
        {
            return query
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.Locality)
                        .ThenInclude(l => l!.State);
        }
    }
}
