using Domain.Products.Entities;
using Domain.Products.Enums;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Products.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_ValidData_FillsProperties()
        {
            Product product = new("Cimento", MeasureUnit.Kilograms);

            product.Description.ShouldBe("Cimento");
            product.MeasureUnit.ShouldBe(MeasureUnit.Kilograms);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_EmptyDescription_ThrowsBusinessRuleException(string description)
        {
            Should.Throw<BusinessRuleException>(() => new Product(description, MeasureUnit.Meters));
        }

        [Fact]
        public void Constructor_InvalidMeasureUnit_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new Product("Cimento", (MeasureUnit)99));
        }

        [Fact]
        public void Fill_ValidData_UpdatesProperties()
        {
            Product product = new("Cimento", MeasureUnit.Kilograms);

            product.Fill("Areia", MeasureUnit.MetersCubed);

            product.Description.ShouldBe("Areia");
            product.MeasureUnit.ShouldBe(MeasureUnit.MetersCubed);
        }
    }
}
