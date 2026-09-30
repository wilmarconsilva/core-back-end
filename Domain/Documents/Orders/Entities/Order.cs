using Domain.Documents.Budgets.Entities;
using Domain.Documents.Entities;
using Domain.Documents.Orders.ValueObjects;

namespace Domain.Documents.Orders.Entities
{
    public class Order : Document<OrderProduct, OrderPayment>
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public Order() { }

        public Order(long userId, DateTime creationDateTime, long customerId, OrderPayment payment, string? observations, byte[]? customerSignatureImage, byte[]? pdf, IReadOnlyCollection<OrderProduct> products, long? budgetId)
            : base(userId, creationDateTime, customerId, payment, observations, customerSignatureImage, pdf, products)
        {
            BudgetId = budgetId;
        }

        public long? BudgetId { get; set; }

        public Budget? Budget { get; private set; }
    }
}
