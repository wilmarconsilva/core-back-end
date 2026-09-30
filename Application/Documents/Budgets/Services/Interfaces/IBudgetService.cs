using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Shared.Services.Interfaces;

namespace Application.Documents.Budgets.Services.Interfaces
{
    public interface IBudgetService : ICrudService<BudgetSaveDto, BudgetDetailDto, BudgetListing, BudgetListingFilter>
    {

    }
}
