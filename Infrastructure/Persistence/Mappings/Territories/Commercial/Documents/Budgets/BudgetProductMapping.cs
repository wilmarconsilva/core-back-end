using Domain.Entities.Commercial.Documents.Budgets;

namespace Infrastructure.Persistence.Mappings.Territories.Commercial.Documents.Budgets
{
    public class BudgetProductMapping : DocumentProductMapping<BudgetProduct>
    {
        protected override string GetTableName()
        {
            return "budgets_products";
        }
    }
}
