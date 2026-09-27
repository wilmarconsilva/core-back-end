using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Listings;

namespace Application.Interfaces.Services.Commercial.Documents.Orders
{
    public interface IOrderService : ICrudService<OrderSaveDto, OrderDetailDto, OrderListing, OrderListingFilter>
    {

    }
}
