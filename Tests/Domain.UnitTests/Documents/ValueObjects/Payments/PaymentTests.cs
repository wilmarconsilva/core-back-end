using Domain.Documents.Budgets.ValueObjects;
using Domain.Documents.Orders.ValueObjects;
using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Exceptions;

namespace Domain.UnitTests.Documents.ValueObjects.Payments
{
    public class PaymentTests
    {
        [Fact]
        public void BudgetPayment_WithoutMethod_IsAllowed()
        {
            BudgetPayment payment = new(null, 0, null);

            payment.Method.ShouldBeNull();
            payment.HasInstallment.ShouldBeFalse();
        }

        [Fact]
        public void BudgetPayment_InvalidMethod_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new BudgetPayment((PaymentMethod)99, 0, null));
        }

        [Fact]
        public void OrderPayment_InvalidMethod_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new OrderPayment((PaymentMethod)0, 0, null));
        }

        [Fact]
        public void OrderPayment_NegativeDiscount_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => new OrderPayment(PaymentMethod.PIX, -10, null));

            exception.Message.ShouldBe("O campo: O valor do desconto não pode ser negativo.");
        }

        [Fact]
        public void BudgetPayment_NegativeDiscount_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => new BudgetPayment(null, -10, null));
        }

        [Fact]
        public void OrderPayment_ZeroDiscount_IsAllowed()
        {
            OrderPayment payment = new(PaymentMethod.PIX, 0, null);

            payment.DiscountValue.ShouldBe(0);
        }

        [Fact]
        public void OrderPayment_WithInstallment_FlagsInstallment()
        {
            Installment installment = new(2, null);

            OrderPayment payment = new(PaymentMethod.CreditCard, 5, installment);

            payment.Method.ShouldBe(PaymentMethod.CreditCard);
            payment.DiscountValue.ShouldBe(5);
            payment.HasInstallment.ShouldBeTrue();
            payment.Installment.ShouldBeSameAs(installment);
        }
    }
}
