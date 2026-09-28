using Application.Shared.DataTransferObjects.Entities;

namespace Application.Persons.Customers.DataTransferObjects.CustomerAddresses
{
    public class CustomerAddressSaveDto : EntitySaveDto
    {
        public required string Neighborhood { get; set; }

        public required string Street { get; set; }

        public required long LocalityId { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }

        public string? Number { get; set; }
    }
}
