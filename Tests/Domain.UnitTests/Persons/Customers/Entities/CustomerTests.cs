using Domain.Persons.Customers.Entities;
using Domain.Persons.Enums;
using Domain.Shared.Exceptions;
using Domain.UnitTests.Builders;

namespace Domain.UnitTests.Persons.Customers.Entities
{
    public class CustomerTests
    {
        [Fact]
        public void Constructor_NaturalPerson_UsesCpf()
        {
            Customer customer = CustomerBuilder.Customer(personType: PersonType.Natural, document: CustomerBuilder.ValidCpf);

            customer.DocumentType.ShouldBe(DocumentType.CPF);
            customer.Document.ShouldBe(CustomerBuilder.ValidCpf);
            customer.Description.ShouldBe("João da Silva");
            customer.Addresses.Count.ShouldBe(1);
        }

        [Fact]
        public void Constructor_LegalPerson_UsesCnpj()
        {
            Customer customer = CustomerBuilder.Customer(personType: PersonType.Legal, document: CustomerBuilder.ValidCnpj);

            customer.DocumentType.ShouldBe(DocumentType.CNPJ);
        }

        [Fact]
        public void Constructor_InvalidPersonType_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => CustomerBuilder.Customer(personType: (PersonType)99));
        }

        [Fact]
        public void Constructor_InvalidCpf_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => CustomerBuilder.Customer(document: "123.456.789-00"));

            exception.Message.ShouldBe("O campo: CPF contém um CPF inválido.");
        }

        [Fact]
        public void Constructor_EmptyName_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => CustomerBuilder.Customer(name: ""));
        }

        [Fact]
        public void Constructor_InvalidEmail_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => CustomerBuilder.Customer(email: "email-invalido"));
        }

        [Fact]
        public void Constructor_WithoutEmail_IsAllowed()
        {
            Customer customer = CustomerBuilder.Customer(email: null);

            customer.Email.ShouldBeNull();
        }

        [Fact]
        public void Constructor_WithoutAddresses_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => CustomerBuilder.Customer(addresses: []));

            exception.Message.ShouldBe("O cliente: João da Silva deve possuir ao menos um endereço.");
        }

        [Fact]
        public void Fill_ExistingCustomer_SynchronizesAddresses()
        {
            Customer customer = CustomerBuilder.Customer(addresses: [CustomerBuilder.Address(street: "Rua A"), CustomerBuilder.Address(street: "Rua B")]);
            customer.Id = 7;
            customer.Addresses.First(a => a.Street == "Rua A").Id = 1;
            customer.Addresses.First(a => a.Street == "Rua B").Id = 2;

            customer.Fill("João da Silva", PersonType.Natural, CustomerBuilder.ValidCpf, null, null,
                [CustomerBuilder.Address(street: "Rua A alterada", id: 1), CustomerBuilder.Address(street: "Rua C")]);

            customer.Addresses.Count.ShouldBe(2);
            customer.Addresses.ShouldContain(a => a.Id == 1 && a.Street == "Rua A alterada");
            customer.Addresses.ShouldContain(a => a.Id == 0 && a.Street == "Rua C" && a.CustomerId == 7);
            customer.Addresses.ShouldNotContain(a => a.Id == 2);
        }

        [Fact]
        public void Fill_NullEmail_ClearsExistingEmail()
        {
            Customer customer = CustomerBuilder.Customer(email: "joao@email.com");

            customer.Fill("João da Silva", PersonType.Natural, CustomerBuilder.ValidCpf, null, null, [CustomerBuilder.Address()]);

            customer.Email.ShouldBeNull();
        }
    }
}
