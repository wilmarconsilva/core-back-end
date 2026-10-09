using Domain.Persons.Customers.Entities;
using Domain.Persons.Enums;

namespace Domain.UnitTests.Builders
{
    internal static class CustomerBuilder
    {
        public const string ValidCpf = "529.982.247-25";
        public const string ValidCnpj = "11.222.333/0001-81";

        public static CustomerAddress Address(long localityId = 1, string street = "Rua A", long id = 0)
        {
            return new CustomerAddress(localityId, "Centro", street, null, null, "100") { Id = id };
        }

        public static Customer Customer(
            string name = "João da Silva",
            PersonType personType = PersonType.Natural,
            string document = ValidCpf,
            string? email = "joao@email.com",
            IReadOnlyCollection<CustomerAddress>? addresses = null)
        {
            return new Customer(name, personType, document, email, null, addresses ?? [Address()]);
        }
    }
}
