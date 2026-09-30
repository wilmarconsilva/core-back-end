using Application.Persons.Customers.DataTransferObjects.CustomerAddresses;
using Application.Shared.DataTransferObjects.Entities;
using Domain.Persons.Enums;

namespace Application.Persons.Customers.DataTransferObjects
{
    public class CustomerSaveDto : EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public override long Id { get; set; }

        public required string Name { get; set; }
        
        public required PersonType PersonType { get; set; }

        public required string Document { get; set; }

        public required List<CustomerAddressSaveDto> Addresses { get; set; }

        public string? Email { get; set; }

        public string? StateRegistration { get; set; }
    }
}