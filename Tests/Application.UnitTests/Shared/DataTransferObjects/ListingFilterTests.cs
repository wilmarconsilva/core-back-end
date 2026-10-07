using Application.Products.DataTransferObjects.Listings;
using Domain.Shared.Exceptions;

namespace Application.UnitTests.Shared.DataTransferObjects
{
    public class ListingFilterTests
    {
        [Fact]
        public void Constructor_Default_ReturnsFirstPageWithMaxQuantity()
        {
            ProductListingFilter filter = new();

            filter.RegistersStart.ShouldBe(0);
            filter.RegistersEnd.ShouldBe(50);
            filter.RegistersQuantity.ShouldBe(50);
        }

        [Fact]
        public void Constructor_NegativeStart_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new ProductListingFilter(-1, 10));
        }

        [Fact]
        public void Constructor_EndLessThanOne_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new ProductListingFilter(0, 0));
        }

        [Fact]
        public void RegistersQuantity_AboveMaxLimit_ThrowsBusinessRuleException()
        {
            ProductListingFilter filter = new(0, 51);

            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => filter.RegistersQuantity);

            exception.Message.ShouldBe("A quantidade de registros a ser retornada: 51 excede a máxima permitida. A máxima permitida é de: 50");
        }
    }
}
