using Application.DataTransferObjects.Commercial.Documents.Listings;

namespace Application.DataTransferObjects.Commercial.Documents.Orders.Listings
{
    public class OrderListing : DocumentListing
    {
        public long? BudgetId { get; init; }
    }
}
