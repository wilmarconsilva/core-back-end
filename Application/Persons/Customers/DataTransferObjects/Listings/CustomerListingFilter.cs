using Application.Shared.DataTransferObjects.Entities;

namespace Application.Persons.Customers.DataTransferObjects.Listings
{
    public class CustomerListingFilter : EntityListingFilter
    {
        public CustomerListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public CustomerListingFilter() : base() { }

        public string? Name { get; init; }
    }
}
