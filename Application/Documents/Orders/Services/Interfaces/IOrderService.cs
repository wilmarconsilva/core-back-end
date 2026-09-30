using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Listings;
using Application.Shared.Services.Interfaces;

namespace Application.Documents.Orders.Services.Interfaces
{
    public interface IOrderService : ICrudService<OrderSaveDto, OrderDetailDto, OrderListing, OrderListingFilter>
    {

    }
}
