using Application.DataTransferObjects.Persons.Customers;
using Application.DataTransferObjects.Persons.Customers.Listings;

namespace Application.Interfaces.Services.Persons
{
    public interface ICustomerService : ICrudService<CustomerSaveDto, CustomerDetailDto, CustomerListing, CustomerListingFilter>
    {
    }
}
