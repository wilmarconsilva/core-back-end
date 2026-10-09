using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.ValueObjects;
using Domain.Documents.Orders.Entities;
using Domain.Documents.Orders.ValueObjects;
using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Documents.ValueObjects.Payments.Installments;

namespace Domain.UnitTests.Builders
{
    internal static class DocumentBuilder
    {
        public const long SellerId = 1;
        public const long CustomerId = 2;

        public static readonly DateTime CreationDateTime = new(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc);

        public static OrderProduct OrderProduct(long productId = 10, decimal quantity = 2, decimal valueUnit = 50, long id = 0)
        {
            return new OrderProduct(productId, quantity, valueUnit, quantity * valueUnit) { Id = id };
        }

        public static BudgetProduct BudgetProduct(long productId = 10, decimal quantity = 2, decimal valueUnit = 50, long id = 0)
        {
            return new BudgetProduct(productId, quantity, valueUnit, quantity * valueUnit) { Id = id };
        }

        public static OrderPayment OrderPayment(decimal discountValue = 0, Installment? installment = null, PaymentMethod method = PaymentMethod.PIX)
        {
            return new OrderPayment(method, discountValue, installment);
        }

        public static BudgetPayment BudgetPayment(decimal discountValue = 0, Installment? installment = null, PaymentMethod? method = PaymentMethod.PIX)
        {
            return new BudgetPayment(method, discountValue, installment);
        }

        public static Order Order(
            OrderPayment? payment = null,
            IReadOnlyCollection<OrderProduct>? products = null,
            long sellerId = SellerId,
            long customerId = CustomerId,
            long? budgetId = null)
        {
            return new Order(sellerId, CreationDateTime, customerId, payment ?? OrderPayment(), "Observação", null, null, products ?? [OrderProduct()], budgetId);
        }

        public static Budget Budget(
            BudgetPayment? payment = null,
            IReadOnlyCollection<BudgetProduct>? products = null,
            long sellerId = SellerId,
            long customerId = CustomerId)
        {
            return new Budget(sellerId, CreationDateTime, customerId, payment ?? BudgetPayment(), "Observação", null, null, products ?? [BudgetProduct()]);
        }
    }
}
