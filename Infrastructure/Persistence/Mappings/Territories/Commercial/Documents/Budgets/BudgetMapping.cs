using Domain.Entities.Commercial.Documents.Budgets;

namespace Infrastructure.Persistence.Mappings.Territories.Commercial.Documents.Budgets
{
    public class BudgetMapping : DocumentMapping<Budget, BudgetProduct>
    {
        protected override string GetTableName()
        {
            return "budgets";
        }
    }
}