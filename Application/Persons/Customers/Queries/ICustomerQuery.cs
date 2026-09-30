using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Application.Shared.Queries.Interfaces;

namespace Application.Persons.Customers.Queries
{
    public interface ICustomerQuery : IEntityCrudQuery<CustomerDetailDto, CustomerListing, CustomerListingFilter>
    {
    }
}
