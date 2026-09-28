using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;

namespace Application.Interfaces.Services.Persons
{
    public interface ICustomerService : ICrudService<CustomerSaveDto, CustomerDetailDto, CustomerListing, CustomerListingFilter>
    {
    }
}
