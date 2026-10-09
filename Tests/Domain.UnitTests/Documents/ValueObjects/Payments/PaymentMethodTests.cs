using Domain.Documents.ValueObjects.Payments.Enums;

namespace Domain.UnitTests.Documents.ValueObjects.Payments
{
    public class PaymentMethodTests
    {
        [Theory]
        [InlineData(PaymentMethod.DebitCard, "Cartão de Débito")]
        [InlineData(PaymentMethod.CreditCard, "Cartão de Crédito")]
        [InlineData(PaymentMethod.Money, "Dinheiro")]
        [InlineData(PaymentMethod.Deposit, "Depósito")]
        [InlineData(PaymentMethod.Slip, "Boleto")]
        [InlineData(PaymentMethod.Check, "Cheque")]
        [InlineData(PaymentMethod.PIX, "PIX")]
        public void GetDescription_DefinedMethod_ReturnsPortugueseDescription(PaymentMethod method, string expected)
        {
            method.GetDescription().ShouldBe(expected);
        }

        [Fact]
        public void GetDescription_UndefinedMethod_ReturnsNotDefined()
        {
            ((PaymentMethod?)null).GetDescription().ShouldBe("Não definida");
        }
    }
}
