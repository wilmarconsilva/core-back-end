using Application.Shared.DataTransferObjects.Entities;
using Domain.Persons.Enums;

namespace Application.Persons.Customers.DataTransferObjects.Synchronizations
{
    /// <summary>
    /// Cliente para o snapshot offline do front-end, no mesmo formato do CustomerSaveDto (com Id).
    /// </summary>
    public class CustomerSynchronizationDto : EntityListing
    {
        public required string Name { get; init; }

        public required PersonType PersonType { get; init; }

        public required string Document { get; init; }

        public required List<CustomerAddressSynchronizationDto> Addresses { get; init; }

        public string? Email { get; init; }

        public string? StateRegistration { get; init; }
    }
}
