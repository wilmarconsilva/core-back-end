using Domain.ValueObjects.Commercial.Documents.Payments;

namespace Domain.Entities.Commercial.Documents.Budgets
{
    public class Budget : Document<BudgetProduct>
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public Budget() { }

        public Budget(long userId, long customerId, Payment payment, string? observations, byte[]? customerSignatureImage, IReadOnlyCollection<BudgetProduct> products)
            : base(userId, customerId, payment, observations, customerSignatureImage, products)
        {
        }
    }
}
