using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Documents.DataTransferObjects;

namespace Application.Interfaces.Services.Commercial.Documents
{
    public interface IBudgetService : ICrudService<BudgetSaveDto, DocumentDetailDto, BudgetListing, BudgetListingFilter>
    {

    }
}
