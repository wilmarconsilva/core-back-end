using Application.DataTransferObjects.Commercial.Documents.Orders;
using Domain.Entities.Commercial.Documents.Budgets;

namespace Application.Interfaces.Services.Commercial.Documents.Orders
{
    public interface IOrderService : ICrudService<OrderSaveDto>
    {
        Task<OrderSaveDto> CreateByBudgetAsync(Budget budget, CancellationToken cancellationToken);
    }
}
