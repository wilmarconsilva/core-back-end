using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Documents.Budgets.Repositories
{
    public class BudgetRepository : DocumentRepository<Budget, BudgetProduct>, IBudgetRepository
    {
        public BudgetRepository(AppDbContext context) : base(context)
        {
        }
    }
}
