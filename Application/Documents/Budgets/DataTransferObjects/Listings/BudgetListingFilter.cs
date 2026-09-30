using Application.Documents.DataTransferObjects.Listings;

namespace Application.Documents.Budgets.DataTransferObjects.Listings
{
    public class BudgetListingFilter : DocumentListingFilter
    {
        public BudgetListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public BudgetListingFilter() : base() { }
    }
}
