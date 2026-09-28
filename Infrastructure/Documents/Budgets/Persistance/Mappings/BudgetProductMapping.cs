using Domain.Documents.Budgets.Entities;
using Infrastructure.Documents.Persistance.Mappings;

namespace Infrastructure.Documents.Budgets.Persistance.Mappings
{
    public class BudgetProductMapping : DocumentProductMapping<BudgetProduct>
    {
        protected override string GetTableName()
        {
            return "budgets_products";
        }
    }
}
