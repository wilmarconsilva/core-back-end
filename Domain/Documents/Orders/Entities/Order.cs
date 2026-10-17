using Domain.Documents.Budgets.Entities;
using Domain.Documents.Entities;
using Domain.Documents.ValueObjects.Payments;

namespace Domain.Documents.Orders.Entities
{
    public class Order : Document<OrderProduct>
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public Order() { }

        public Order(long userId, DateTime creationDateTime, long customerId, Payment payment, string? observations, byte[]? customerSignatureImage, byte[]? pdf, IReadOnlyCollection<OrderProduct> products, long? budgetId)
            : base(userId, creationDateTime, customerId, payment, observations, customerSignatureImage, pdf, products)
        {
            BudgetId = budgetId;
        }

        public long? BudgetId { get; set; }

        public Budget? Budget { get; private set; }
    }
}
