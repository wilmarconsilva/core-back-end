using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Documents.ValueObjects.Payments
{
    public class DownPaymentTests
    {
        [Theory]
        [InlineData(0, PaymentMethod.PIX)]
        [InlineData(10, (PaymentMethod)0)]
        [InlineData(-10, PaymentMethod.PIX)]
        public void Constructor_InvalidValue_ThrowsBusinessRuleException(decimal value, PaymentMethod method)
        {
            Should.Throw<BusinessRuleException>(() => new DownPayment(value, method));
        }

        [Fact]
        public void Constructor_ValueWithMoreThanTwoDecimals_RoundsToCents()
        {
            DownPayment downPayment = new(10.555m, PaymentMethod.PIX);

            downPayment.Value.ShouldBe(10.56m);
        }

        [Theory]
        [InlineData(10, 100, 10)]
        [InlineData(15000, 26000, 57.69)]
        [InlineData(1, 3, 33.33)]
        [InlineData(2, 3, 66.67)]
        public void SetPercentage_ValidTotal_DerivesRoundedPercentageFromValue(decimal value, decimal valueTotal, decimal expected)
        {
            DownPayment downPayment = new(value, PaymentMethod.PIX);

            downPayment.SetPercentage(valueTotal);

            downPayment.Percentage.ShouldBe(expected);
        }

        [Fact]
        public void SetPercentage_ZeroTotal_ThrowsBusinessRuleException()
        {
            DownPayment downPayment = new(10, PaymentMethod.PIX);

            Should.Throw<BusinessRuleException>(() => downPayment.SetPercentage(0));
        }

        [Theory]
        [InlineData(99.99, true)]
        [InlineData(100, false)]
        public void ValidateValue_ReturnsWhetherValueIsLessThanTotal(decimal value, bool expected)
        {
            DownPayment downPayment = new(value, PaymentMethod.PIX);

            downPayment.ValidateValue(100).ShouldBe(expected);
        }
    }
}
