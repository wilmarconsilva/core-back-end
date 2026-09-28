using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Repositories.Commercial.Documents.Budgets
{
    public class BudgetRepository : DocumentoRepository<Budget, BudgetProduct>, IBudgetRepository
    {
        public BudgetRepository(AppDbContext context) : base(context)
        {
        }
    }
}
