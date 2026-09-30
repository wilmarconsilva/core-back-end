using Application.Documents.DataTransferObjects.Listings;

namespace Application.Documents.Orders.DataTransferObjects.Listings
{
    public class OrderListingFilter : DocumentListingFilter
    {
        public OrderListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public OrderListingFilter() : base() { }
    }
}
