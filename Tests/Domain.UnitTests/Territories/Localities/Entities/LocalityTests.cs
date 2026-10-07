using Domain.Shared.Exceptions;
using Domain.Territories.Localities.Entities;

namespace Domain.UnitTests.Territories.Localities.Entities
{
    public class LocalityTests
    {
        [Fact]
        public void Constructor_ValidData_FillsProperties()
        {
            Locality locality = new("Curitiba", 41);

            locality.Description.ShouldBe("Curitiba");
            locality.StateId.ShouldBe(41);
        }

        [Fact]
        public void Constructor_WithoutState_IsAllowed()
        {
            Locality locality = new("Curitiba", null);

            locality.StateId.ShouldBeNull();
        }

        [Fact]
        public void Constructor_EmptyDescription_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new Locality("", 41));
        }
    }
}
