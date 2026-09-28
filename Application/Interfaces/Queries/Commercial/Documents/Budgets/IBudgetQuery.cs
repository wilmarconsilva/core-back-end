using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Documents.DataTransferObjects;

namespace Application.Interfaces.Queries.Commercial.Documents.Budgets
{
    public interface IBudgetQuery : IEntityCrudQuery<DocumentDetailDto, BudgetListing, BudgetListingFilter>
    {
    }
}
