using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Listings;

namespace Application.Interfaces.Queries.Commercial.Documents.Orders
{
    public interface IOrderQuery : IEntityCrudQuery<OrderDetailDto, OrderListing, OrderListingFilter>
    {
    }
}
