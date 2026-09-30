using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Listings;
using Application.Shared.Queries.Interfaces;

namespace Application.Documents.Orders.Queries
{
    public interface IOrderQuery : IEntityCrudQuery<OrderDetailDto, OrderListing, OrderListingFilter>
    {
    }
}
