using Domain.Persons.Customers.Entities;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Persons.Customers.Entities
{
    public class CustomerAddressTests
    {
        [Theory]
        [InlineData("Centro", "Rua A", null, null, null, "Centro, Rua A.")]
        [InlineData("Centro", "Rua A", "100", null, null, "Centro, Rua A - 100.")]
        [InlineData("Centro", "Rua A", "100", "Apto 2", "80000-000", "Centro, Rua A - 100. Apto 2. 80000-000.")]
        [InlineData(null, "Rua A", "100", null, null, "Rua A - 100.")]
        [InlineData("Centro", null, null, null, null, "Centro.")]
        [InlineData(null, null, "100", null, null, "100.")]
        [InlineData(null, null, null, "Apto 2", "80000-000", "Apto 2. 80000-000.")]
        [InlineData(null, null, null, null, null, "")]
        public void Description_OptionalFields_ComposesDescription(string? neighborhood, string? street, string? number, string? complement, string? postalCode, string expected)
        {
            CustomerAddress address = new(1, neighborhood, street, complement, postalCode, number);

            address.Description.ShouldBe(expected);
        }

        [Fact]
        public void Constructor_ZeroLocality_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new CustomerAddress(0, "Centro", "Rua A", null, null, null));
        }

        [Fact]
        public void Constructor_WithoutNeighborhoodAndStreet_CreatesAddress()
        {
            CustomerAddress address = new(1, null, null, null, null, null);

            address.LocalityId.ShouldBe(1);
            address.Neighborhood.ShouldBeNull();
            address.Street.ShouldBeNull();
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
