using Domain.Documents.Entities;

namespace Domain.Documents.Budgets.Entities
{
    public class BudgetProduct : DocumentProduct
    {
        /// <summary>
        /// Constructor only for ORM.
        /// </summary>
        public BudgetProduct() { }

        public BudgetProduct(long productId, decimal quantity, decimal valueUnit)
            : base(productId, quantity, valueUnit)
        {
        }
    }
}