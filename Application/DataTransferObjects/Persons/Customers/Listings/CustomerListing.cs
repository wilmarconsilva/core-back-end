using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Persons.Customers.Listings
{
    public class CustomerListing : EntityListing
    {
        public required string Name { get; init; }

        public required string Document { get; init; }
    }
}
