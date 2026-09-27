using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;

namespace Application.Interfaces.Services.Commercial.Documents
{
    public interface IBudgetService : ICrudService<BudgetSaveDto, DocumentDetailDto, BudgetListing, BudgetListingFilter>
    {

    }
}
