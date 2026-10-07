using Domain.Shared.Exceptions;
using Domain.Territories.Entities;

namespace Domain.UnitTests.Territories.Entities
{
    public class TerritoryTests
    {
        [Fact]
        public void StateConstructor_ValidAcronym_FillsAcronym()
        {
            State state = new("PR");

            state.Acronym.ShouldBe("PR");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void StateConstructor_EmptyAcronym_ThrowsBusinessRuleException(string acronym)
        {
            Should.Throw<BusinessRuleException>(() => new State(acronym));
        }

        [Fact]
        public void CountrySetDescription_ValidDescription_FillsDescription()
        {
            Country country = new("BR");

            country.SetDescription("Brasil");

            country.Description.ShouldBe("Brasil");
        }

        [Fact]
        public void CountrySetDescription_EmptyDescription_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new Country("BR").SetDescription(""));
        }
    }
}
