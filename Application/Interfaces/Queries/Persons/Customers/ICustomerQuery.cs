using Application.DataTransferObjects.Persons.Customers;
using Application.DataTransferObjects.Persons.Customers.Listings;

namespace Application.Interfaces.Queries.Persons.Customers
{
    public interface ICustomerQuery : IEntitySaveDtoQuery<CustomerSaveDto>, IEntityListingQuery<CustomerListing, CustomerListingFilter>
    {

    }
}