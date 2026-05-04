using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Localities;

namespace Application.DataTransferObjects.Persons.Customers
{
    public class CustomerAddressDetailDto : EntitySaveDto
    {
        public required string Neighborhood { get; set; }

        public required string Street { get; set; }

        public required LocalityReferenceDto Locality { get; set; }

        public string? Complement { get; set; }

        public string? PostalCode { get; set; }

        public string? Number { get; set; }
    }
}
