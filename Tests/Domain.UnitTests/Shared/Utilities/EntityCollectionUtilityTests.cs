using Domain.Persons.Customers.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Utilities;
using Domain.UnitTests.Builders;

namespace Domain.UnitTests.Shared.Utilities
{
    public class EntityCollectionUtilityTests
    {
        [Fact]
        public void Sync_ExistingEntityNotIncoming_RemovesIt()
        {
            List<CustomerAddress> current = [CustomerBuilder.Address(id: 1), CustomerBuilder.Address(id: 2)];

            EntityCollectionUtility.Sync(current, [CustomerBuilder.Address(id: 1)], 5);

            current.Select(a => a.Id).ShouldBe([1L]);
        }

        [Fact]
        public void Sync_ExistingEntityIncoming_CopiesOntoTrackedInstance()
        {
            CustomerAddress tracked = CustomerBuilder.Address(street: "Rua A", id: 1);
            List<CustomerAddress> current = [tracked];

            EntityCollectionUtility.Sync(current, [CustomerBuilder.Address(street: "Rua B", id: 1)], 5);

            current.ShouldHaveSingleItem().ShouldBeSameAs(tracked);
            tracked.Street.ShouldBe("Rua B");
        }

        [Fact]
        public void Sync_NewEntityWithPersistedFather_AddsItWithFatherId()
        {
            List<CustomerAddress> current = [];

            EntityCollectionUtility.Sync(current, [CustomerBuilder.Address()], 5);

            current.ShouldHaveSingleItem().CustomerId.ShouldBe(5);
        }

        [Fact]
        public void Sync_NewEntityWithNewFather_AddsItWithoutFatherId()
        {
            List<CustomerAddress> current = [];

            EntityCollectionUtility.Sync(current, [CustomerBuilder.Address()], 0);

            current.ShouldHaveSingleItem().CustomerId.ShouldBe(0);
        }

        [Fact]
        public void Sync_IncomingIdNotInCurrent_ThrowsBusinessRuleException()
        {
            List<CustomerAddress> current = [CustomerBuilder.Address(id: 1)];

            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => EntityCollectionUtility.Sync(current, [CustomerBuilder.Address(id: 99)], 5));

            exception.Message.ShouldBe("O registro de: CustomerAddress com o código: 99 não foi encontrado.");
        }
    }
}
