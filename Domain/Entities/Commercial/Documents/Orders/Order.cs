using Domain.Entities.Commercial.Documents.Budgets;
using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Domain.Entities.Commercial.Documents.Orders
{
    public class Order : Document<OrderProduct>
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public Order() { }

        public Order(long userId, long customerId, Payment payment, string? observations, byte[]? customerSignatureImage, List<OrderProduct> products, long? budgetId)
            : base(userId, customerId, payment, observations, customerSignatureImage, products)
        {
            BudgetId = budgetId;
        }

        public long? BudgetId { get; set; }

        public Budget? Budget { get; private set; }
    }
}
