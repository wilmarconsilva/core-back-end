using Application.DataTransferObjects.Commercial.Documents.Listings;

namespace Application.DataTransferObjects.Commercial.Documents.Budgets.Listings
{
    public class BudgetListingFilter : DocumentListingFilter
    {
        public BudgetListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public BudgetListingFilter() : base() { }
    }
}
