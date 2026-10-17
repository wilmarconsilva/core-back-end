using Domain.Documents.Entities;
using Domain.Documents.ValueObjects.Payments;

namespace Domain.Documents.Budgets.Entities
{
    public class Budget : Document<BudgetProduct>
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public Budget() { }

        public Budget(long userId, DateTime creationDateTime, long customerId, Payment payment, string? observations, byte[]? customerSignatureImage, byte[]? pdf, IReadOnlyCollection<BudgetProduct> products)
            : base(userId, creationDateTime, customerId, payment, observations, customerSignatureImage, pdf, products)
        {
        }
    }
}
