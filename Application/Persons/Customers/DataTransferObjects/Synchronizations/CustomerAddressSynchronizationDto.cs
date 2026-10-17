namespace Application.Persons.Customers.DataTransferObjects.Synchronizations
{
    public class CustomerAddressSynchronizationDto
    {
        public required long Id { get; init; }

        public string? Neighborhood { get; init; }

        public string? Street { get; init; }

        public required long LocalityId { get; init; }

        public required string LocalityDescription { get; init; }

        public string? Complement { get; init; }

        public string? PostalCode { get; init; }

        public string? Number { get; init; }
    }
}
