using Domain.Entities.Commercial.Documents.Budgets;

namespace Infrastructure.Persistence.Mappings.Commercial.Documents.Budgets
{
    public class BudgetMapping : DocumentMapping<Budget, BudgetProduct>
    {
        protected override string GetTableName()
        {
            return "budgets";
        }
    }
}