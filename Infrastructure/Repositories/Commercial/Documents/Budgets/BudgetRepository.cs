using Domain.Entities.Commercial.Documents.Budgets;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;

namespace Infrastructure.Repositories.Commercial.Documents.Budgets
{
    public class BudgetRepository : DocumentoRepository<Budget, BudgetProduct>, IBudgetRepository
    {
        public BudgetRepository(AppDbContext context) : base(context)
        {
        }
    }
}
