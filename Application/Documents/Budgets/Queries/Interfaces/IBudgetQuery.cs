using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Shared.Queries.Interfaces;

namespace Application.Documents.Budgets.Queries.Interfaces
{
    public interface IBudgetQuery : IEntityCrudQuery<BudgetDetailDto, BudgetListing, BudgetListingFilter>
    {
    }
}
