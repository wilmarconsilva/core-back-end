using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Application.Shared.Services.Interfaces;

namespace Application.Persons.Customers.Services.Interfaces
{
    public interface ICustomerService : ICrudService<CustomerSaveDto, CustomerDetailDto, CustomerListing, CustomerListingFilter>
    {
    }
}
