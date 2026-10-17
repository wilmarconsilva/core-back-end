using Application.Shared.DataTransferObjects.Entities;

namespace Application.Persons.Customers.DataTransferObjects.CustomerAddresses
{
    public class CustomerAddressDetailDto : EntitySaveDto
    {
        public string? Neighborhood { get; set; }

        public string? Street { get; set; }

        public required EntityReferenceDto Locality { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }

        public string? Number { get; set; }
    }
}
