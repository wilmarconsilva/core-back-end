using Domain.Documents.ValueObjects.Payments;
using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Documents.ValueObjects.Payments
{
    public class PaymentTests
    {
        [Fact]
        public void Constructor_WithoutMethod_IsAllowed()
        {
            Payment payment = new(null, 0, null);

            payment.Method.ShouldBeNull();
            payment.HasInstallment.ShouldBeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(99)]
        public void Constructor_InvalidMethod_ThrowsBusinessRuleException(int method)
        {
            Should.Throw<BusinessRuleException>(() => new Payment((PaymentMethod)method, 0, null));
        }

        [Fact]
        public void Constructor_NegativeDiscount_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => new Payment(PaymentMethod.PIX, -10, null));

            exception.Message.ShouldBe("O campo: O valor do desconto não pode ser negativo.");
        }

        [Fact]
        public void Constructor_NegativeDiscountWithoutMethod_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new Payment(null, -10, null));
        }

        [Fact]
        public void Constructor_ZeroDiscount_IsAllowed()
        {
            Payment payment = new(PaymentMethod.PIX, 0, null);

            payment.DiscountValue.ShouldBe(0);
        }

        [Fact]
        public void Constructor_WithInstallment_FlagsInstallment()
        {
            Installment installment = new(2, null);

            Payment payment = new(PaymentMethod.CreditCard, 5, installment);

            payment.Method.ShouldBe(PaymentMethod.CreditCard);
            payment.DiscountValue.ShouldBe(5);
            payment.HasInstallment.ShouldBeTrue();
            payment.Installment.ShouldBeSameAs(installment);
        }
    }
}
