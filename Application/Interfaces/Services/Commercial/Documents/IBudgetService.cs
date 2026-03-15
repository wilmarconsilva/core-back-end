using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Orders;

namespace Application.Interfaces.Services.Commercial.Documents
{
    public interface IBudgetService : ICrudService<BudgetSaveDto>
    {
        Task<OrderSaveDto> CreateOrderByBudget(long id, CancellationToken cancellationToken);
    }
}