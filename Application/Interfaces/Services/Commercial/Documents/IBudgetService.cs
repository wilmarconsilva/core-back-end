using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;
using Application.DataTransferObjects.Commercial.Documents.Orders;

namespace Application.Interfaces.Services.Commercial.Documents
{
    public interface IBudgetService : ICrudService<BudgetSaveDto, BudgetDetailDto, BudgetListing, BudgetListingFilter>
    {
        Task<OrderDetailDto> CreateOrderByBudget(long id, CancellationToken cancellationToken);
    }
}
