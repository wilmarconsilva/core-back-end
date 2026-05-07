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

        public Order(long userId, DateTime creationDateTime, long customerId, Payment payment, string? observations, byte[]? customerSignatureImage, IReadOnlyCollection<OrderProduct> products, long? budgetId)
            : base(userId, creationDateTime, customerId, payment, observations, customerSignatureImage, products)
        {
            BudgetId = budgetId;
        }

        public Order(Budget budget, DateTime creationDateTime)
            : base(budget.UserId, creationDateTime, budget.CustomerId, budget.Payment, budget.Observations, budget.CustomerSignatureImage,
                   budget.Products.Select(p => new OrderProduct(p.ProductId, p.Quantity, p.ValueUnit, p.ValueTotal)).ToList())
        {
            BudgetId = budget.Id;
            Budget = budget;
        }

        public long? BudgetId { get; set; }

        public Budget? Budget { get; private set; }
    }
}
