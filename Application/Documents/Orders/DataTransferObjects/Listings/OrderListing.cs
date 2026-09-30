using Application.Documents.DataTransferObjects.Listings;

namespace Application.Documents.Orders.DataTransferObjects.Listings
{
    public class OrderListing : DocumentListing
    {
        public long? BudgetId { get; init; }
    }
}
