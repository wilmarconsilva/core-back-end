using Application.DataTransferObjects.Commercial.Documents.Listings;

namespace Application.DataTransferObjects.Commercial.Documents.Orders.Listings
{
    public class OrderListingFilter : DocumentListingFilter
    {
        public OrderListingFilter(int registersStart, int registersEnd) : base(registersStart, registersEnd) { }

        public OrderListingFilter() : base() { }
    }
}
