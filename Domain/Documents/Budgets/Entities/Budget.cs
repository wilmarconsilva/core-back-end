using Domain.Documents.Budgets.ValueObjects;
using Domain.Documents.Entities;

namespace Domain.Documents.Budgets.Entities
{
    public class Budget : Document<BudgetProduct, BudgetPayment>
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public Budget() { }

        public Budget(long userId, DateTime creationDateTime, long customerId, BudgetPayment payment, string? observations, byte[]? customerSignatureImage, byte[]? pdf, IReadOnlyCollection<BudgetProduct> products)
            : base(userId, creationDateTime, customerId, payment, observations, customerSignatureImage, pdf, products)
        {
        }
    }
}
