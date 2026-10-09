using Domain.Persons.Customers.Entities;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Persons.Customers.Entities
{
    public class CustomerAddressTests
    {
        [Theory]
        [InlineData(null, null, null, "Centro, Rua A.")]
        [InlineData("100", null, null, "Centro, Rua A - 100.")]
        [InlineData("100", "Apto 2", "80000-000", "Centro, Rua A - 100. Apto 2. 80000-000.")]
        public void Description_OptionalFields_ComposesDescription(string? number, string? complement, string? postalCode, string expected)
        {
            CustomerAddress address = new(1, "Centro", "Rua A", complement, postalCode, number);

            address.Description.ShouldBe(expected);
        }

        [Fact]
        public void Constructor_ZeroLocality_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new CustomerAddress(0, "Centro", "Rua A", null, null, null));
        }

        [Fact]
        public void Constructor_EmptyNeighborhood_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new CustomerAddress(1, "", "Rua A", null, null, null));
        }

        [Fact]
        public void Constructor_EmptyStreet_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new CustomerAddress(1, "Centro", "", null, null, null));
        }

        [Fact]
        public void Constructor_ZeroCustomer_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new CustomerAddress(1, 0, "Centro", "Rua A", null, null, null));
        }

        [Fact]
        public void Copy_OtherAddress_CopiesValues()
        {
            CustomerAddress address = new(1, "Centro", "Rua A", null, null, null);

            address.Copy(new CustomerAddress(2, "Batel", "Rua B", "Casa", "80000-000", "10"));

            address.LocalityId.ShouldBe(2);
            address.Neighborhood.ShouldBe("Batel");
            address.Street.ShouldBe("Rua B");
            address.Complement.ShouldBe("Casa");
            address.PostalCode.ShouldBe("80000-000");
            address.Number.ShouldBe("10");
        }
    }
}
