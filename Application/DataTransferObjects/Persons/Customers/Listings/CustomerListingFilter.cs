using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Persons.Customers.Listings
{
    public class CustomerListingFilter : EntityListingFilter
    {
        public CustomerListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public CustomerListingFilter() : base() { }

        public string? Name { get; init; }
    }
}
