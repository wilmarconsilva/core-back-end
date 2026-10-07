using Domain.Documents.Budgets.Entities;
using Domain.Documents.Orders.Entities;
using Domain.Shared.Exceptions;
using Domain.UnitTests.Builders;

namespace Domain.UnitTests.Documents.Entities
{
    public class DocumentProductTests
    {
        [Theory]
        [InlineData(0, 2, 50, 100)]
        [InlineData(10, 0, 50, 0)]
        [InlineData(10, 2, 0, 0)]
        [InlineData(10, 2, 50, 0)]
        public void Constructor_ZeroValue_ThrowsBusinessRuleException(long productId, decimal quantity, decimal valueUnit, decimal valueTotal)
        {
            Should.Throw<BusinessRuleException>(() => new OrderProduct(productId, quantity, valueUnit, valueTotal));
        }

        [Theory]
        [InlineData(-2, -50, 100, "Quantity")]
        [InlineData(-2, 50, -100, "Quantity")]
        [InlineData(2, -50, -100, "ValueUnit")]
        public void Constructor_NegativeValue_ThrowsBusinessRuleException(decimal quantity, decimal valueUnit, decimal valueTotal, string propertyName)
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => new OrderProduct(10, quantity, valueUnit, valueTotal));

            exception.Message.ShouldBe($"O campo: {propertyName} não pode ser negativo.");
        }

        [Fact]
        public void BudgetProductConstructor_NegativeValues_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new BudgetProduct(10, -2, -50, 100));
        }

        [Fact]
        public void Constructor_TotalDifferentFromQuantityTimesUnit_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => new OrderProduct(10, 2, 50, 99));

            exception.Message.ShouldBe("O valor total do produto de código: 10 não corresponde com a sua quantidade multiplicada pelo seu valor unitário.");
        }

        [Fact]
        public void BudgetProductConstructor_InvalidValues_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new BudgetProduct(10, 2, 50, 99));
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
