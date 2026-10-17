using Domain.Documents.Budgets.Entities;
using Domain.Documents.Orders.Entities;
using Domain.Shared.Exceptions;
using Domain.UnitTests.Builders;

namespace Domain.UnitTests.Documents.Entities
{
    public class DocumentProductTests
    {
        [Theory]
        [InlineData(0, 2, 50)]
        [InlineData(10, 0, 50)]
        [InlineData(10, 2, 0)]
        public void Constructor_ZeroValue_ThrowsBusinessRuleException(long productId, decimal quantity, decimal valueUnit)
        {
            Should.Throw<BusinessRuleException>(() => new OrderProduct(productId, quantity, valueUnit));
        }

        [Theory]
        [InlineData(-2, -50, "Quantity")]
        [InlineData(-2, 50, "Quantity")]
        [InlineData(2, -50, "ValueUnit")]
        public void Constructor_NegativeValue_ThrowsBusinessRuleException(decimal quantity, decimal valueUnit, string propertyName)
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => new OrderProduct(10, quantity, valueUnit));

            exception.Message.ShouldBe($"O campo: {propertyName} não pode ser negativo.");
        }

        [Fact]
        public void BudgetProductConstructor_NegativeValues_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new BudgetProduct(10, -2, -50));
        }

        [Theory]
        [InlineData(2, 50, 100)]
        [InlineData(1.5, 10.25, 15.375)]
        public void Constructor_ValidValues_CalculatesValueTotal(decimal quantity, decimal valueUnit, decimal expected)
        {
            OrderProduct product = new(10, quantity, valueUnit);

            product.ValueTotal.ShouldBe(expected);
        }

        [Fact]
        public void BudgetProductConstructor_ValidValues_CalculatesValueTotal()
        {
            BudgetProduct product = new(10, 3, 20);

            product.ValueTotal.ShouldBe(60);
        }

        [Fact]
        public void Copy_OtherProduct_CopiesValues()
        {
            OrderProduct product = DocumentBuilder.OrderProduct();

            product.Copy(DocumentBuilder.OrderProduct(productId: 20, quantity: 3, valueUnit: 10));

            product.ProductId.ShouldBe(20);
            product.Quantity.ShouldBe(3);
            product.ValueUnit.ShouldBe(10);
            product.ValueTotal.ShouldBe(30);
        }

        [Fact]
        public void SetFatherEntityId_ValidId_SetsDocumentId()
        {
            OrderProduct product = DocumentBuilder.OrderProduct();

            product.SetFatherEntityId(7);

            product.DocumentId.ShouldBe(7);
        }

        [Fact]
        public void SetFatherEntityId_Zero_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => DocumentBuilder.OrderProduct().SetFatherEntityId(0));
        }
    }
}
