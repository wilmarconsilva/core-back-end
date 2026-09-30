using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.ValueObjects;
using Domain.Documents.Budgets.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Documents.Budgets.Repositories
{
    public class BudgetRepository : DocumentoRepository<Budget, BudgetProduct, BudgetPayment>, IBudgetRepository
    {
        public BudgetRepository(AppDbContext context) : base(context)
        {
        }
    }
}
