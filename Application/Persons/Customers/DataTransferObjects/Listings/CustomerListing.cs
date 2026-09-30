using Application.Shared.DataTransferObjects.Entities;

namespace Application.Persons.Customers.DataTransferObjects.Listings
{
    public class CustomerListing : EntityListing
    {
        public required string Name { get; init; }

        public required string LocalityState { get; init; }
    }
}