using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Documents.ValueObjects.Payments
{
    public class InstallmentTests
    {
        [Fact]
        public void Constructor_ZeroQuantity_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new Installment(0, null));
        }

        [Fact]
        public void Constructor_NegativeQuantity_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => new Installment(-2, null));

            exception.Message.ShouldBe("O campo: A quantidade de parcelas não pode ser negativo.");
        }

        [Fact]
        public void Constructor_WithDownPayment_FlagsDownPayment()
        {
            Installment installment = new(2, new DownPayment(10, 10, PaymentMethod.PIX));

            installment.HasDownPayment.ShouldBeTrue();
        }

        [Fact]
        public void SetValue_WithoutDownPayment_DividesTotalByQuantity()
        {
            Installment installment = new(4, null);

            installment.SetValue(100);

            installment.Value.ShouldBe(25);
        }

        [Fact]
        public void SetValue_WithDownPayment_DividesRemainingByQuantity()
        {
            Installment installment = new(4, new DownPayment(20, 20, PaymentMethod.Money));

            installment.SetValue(100);

            installment.Value.ShouldBe(20);
        }

        [Theory]
        [InlineData(100, 3, 33.33)]
        [InlineData(200, 3, 66.67)]
        [InlineData(10, 7, 1.43)]
        public void SetValue_NonExactDivision_RoundsToCents(decimal valueTotal, int quantity, decimal expected)
        {
            Installment installment = new(quantity, null);

            installment.SetValue(valueTotal);

            installment.Value.ShouldBe(expected);
        }

        [Fact]
        public void SetValue_DownPaymentEqualToTotal_ThrowsBusinessRuleException()
        {
            Installment installment = new(2, new DownPayment(100, 100, PaymentMethod.PIX));

            Should.Throw<BusinessRuleException>(() => installment.SetValue(100));
        }

        [Fact]
        public void SetValue_DownPaymentGreaterThanTotal_ThrowsBusinessRuleException()
        {
            Installment installment = new(2, new DownPayment(150, 150, PaymentMethod.PIX));

            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => installment.SetValue(100));

            exception.Message.ShouldBe("O valor da entrada não pode ser igual ou maior que o valor total do documento.");
        }

        [Fact]
        public void SetValue_DownPaymentPercentageMismatch_ThrowsBusinessRuleException()
        {
            Installment installment = new(2, new DownPayment(30, 20, PaymentMethod.PIX));

            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => installment.SetValue(100));

            exception.Message.ShouldBe("O valor da entrada não corresponde a porcentagem da entrada em relação ao valor total do documento.");
        }
    }
}
