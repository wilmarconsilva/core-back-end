using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;

namespace Application.Interfaces.Queries.Persons.Customers
{
    public interface ICustomerQuery : IEntityCrudQuery<CustomerDetailDto, CustomerListing, CustomerListingFilter>
    {
    }
}
