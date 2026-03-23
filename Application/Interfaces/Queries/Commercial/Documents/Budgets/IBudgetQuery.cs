using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;

namespace Application.Interfaces.Queries.Commercial.Documents.Budgets
{
    public interface IBudgetQuery : IEntitySaveDtoQuery<BudgetSaveDto>, IEntityListingQuery<BudgetListing, BudgetListingFilter>
    {

    }
}
