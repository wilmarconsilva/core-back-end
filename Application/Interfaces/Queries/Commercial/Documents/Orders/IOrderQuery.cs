using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;

namespace Application.Interfaces.Queries.Commercial.Documents.Orders
{
    public interface IOrderQuery : IEntityCrudQuery<OrderDetailDto, OrderListing, OrderListingFilter>
    {
    }
}
