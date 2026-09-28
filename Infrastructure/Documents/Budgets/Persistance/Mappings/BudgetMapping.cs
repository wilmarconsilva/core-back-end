using Domain.Documents.Budgets.Entities;
using Infrastructure.Documents.Persistance.Mappings;

namespace Infrastructure.Documents.Budgets.Persistance.Mappings
{
    public class BudgetMapping : DocumentMapping<Budget, BudgetProduct>
    {
        protected override string GetTableName()
        {
            return "budgets";
        }
    }
}