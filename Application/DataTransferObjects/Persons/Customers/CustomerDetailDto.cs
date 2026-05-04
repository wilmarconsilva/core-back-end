using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Persons.Customers.CustomerAddresses;
using Domain.ValueObjects.Persons.Enums;

namespace Application.DataTransferObjects.Persons.Customers
{
    public class CustomerDetailDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required string Name { get; set; }

        public required PersonType PersonType { get; set; }

        public required string Document { get; set; }

        public required List<CustomerAddressDetailDto> Addresses { get; set; }

        public string? Email { get; set; }

        public string? StateRegistration { get; set; }
    }
}
