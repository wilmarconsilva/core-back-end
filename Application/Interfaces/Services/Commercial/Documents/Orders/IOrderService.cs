using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;
using Domain.Entities.Commercial.Documents.Budgets;

namespace Application.Interfaces.Services.Commercial.Documents.Orders
{
    public interface IOrderService : ICrudService<OrderSaveDto, OrderListing, OrderListingFilter>
    {
        Task<OrderSaveDto> CreateByBudgetAsync(Budget budget, CancellationToken cancellationToken);
    }
}
