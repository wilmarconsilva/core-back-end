using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Documents.ValueObjects.Payments
{
    public class DownPaymentTests
    {
        [Theory]
        [InlineData(0, 10, PaymentMethod.PIX)]
        [InlineData(10, 0, PaymentMethod.PIX)]
        [InlineData(10, 10, (PaymentMethod)0)]
        [InlineData(-10, 10, PaymentMethod.PIX)]
        [InlineData(10, -10, PaymentMethod.PIX)]
        public void Constructor_InvalidValue_ThrowsBusinessRuleException(decimal value, decimal percentage, PaymentMethod method)
        {
            Should.Throw<BusinessRuleException>(() => new DownPayment(value, percentage, method));
        }

        [Theory]
        [InlineData(10, 10, 100, true)]
        [InlineData(10, 10, 99.99, true)]
        [InlineData(15, 10, 100, false)]
        public void ValidatePercentage_ComparesRoundedPercentageOfTotal(decimal value, decimal percentage, decimal valueTotal, bool expected)
        {
            DownPayment downPayment = new(value, percentage, PaymentMethod.PIX);

            downPayment.ValidatePercentage(valueTotal).ShouldBe(expected);
        }

        [Theory]
        [InlineData(99.99, true)]
        [InlineData(100, false)]
        public void ValidateValue_ReturnsWhetherValueIsLessThanTotal(decimal value, bool expected)
        {
            DownPayment downPayment = new(value, 10, PaymentMethod.PIX);

            downPayment.ValidateValue(100).ShouldBe(expected);
        }
    }
}
