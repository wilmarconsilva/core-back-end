using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Listings;

namespace Application.Interfaces.Services.Commercial.Documents.Orders
{
    public interface IOrderService : ICrudService<OrderSaveDto, OrderDetailDto, OrderListing, OrderListingFilter>
    {

    }
}
