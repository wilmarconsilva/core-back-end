using Application.DataTransferObjects.Entities;

namespace Application.DataTransferObjects.Persons.Customers.CustomerAddresses
{
    public class CustomerAddressDetailDto : EntitySaveDto
    {
        public required string Neighborhood { get; set; }

        public required string Street { get; set; }

        public required EntityReferenceDto Locality { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }

        public string? Number { get; set; }
    }
}
