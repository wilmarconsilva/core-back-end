using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;

namespace Application.Interfaces.Queries.Commercial.Documents.Budgets
{
    public interface IBudgetQuery : IEntityCrudQuery<DocumentDetailDto, BudgetListing, BudgetListingFilter>
    {
    }
}
