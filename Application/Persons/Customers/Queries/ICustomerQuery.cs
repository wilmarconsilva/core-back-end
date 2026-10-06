using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Application.Persons.Customers.DataTransferObjects.Synchronizations;
using Application.Shared.Queries.Interfaces;

namespace Application.Persons.Customers.Queries
{
    public interface ICustomerQuery : IEntityCrudQuery<CustomerDetailDto, CustomerListing, CustomerListingFilter>
    {
        Task<List<CustomerSynchronizationDto>> GetSynchronization(CustomerListingFilter filter, CancellationToken cancellationToken);
    }
}
