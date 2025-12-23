using Domain.ValueObjects.Persons.Enums;

namespace Application.DataTransferObjects.Persons.Customers
{
    public class CustomerSaveDto : EntitySaveDto
    {
        public required string Name { get; set; }
        
        public required PersonType PersonType { get; set; }

        public required string Document { get; set; }

        public required List<CustomerAddressSaveDto> Addresses { get; set; }

        public string? Email { get; set; }

        public string? StateRegistration { get; set; }
    }
}