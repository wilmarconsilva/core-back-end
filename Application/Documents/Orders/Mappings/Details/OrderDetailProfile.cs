using Application.Documents.Mappings;
using Application.Documents.Orders.DataTransferObjects;
using Domain.Documents.Orders.Entities;
using Domain.Documents.Orders.ValueObjects;

namespace Application.Documents.Orders.Mappings.Details
{
    public class OrderDetailProfile : DocumentDetailProfile<Order, OrderProduct, OrderPayment, OrderDetailDto>
    {

    }
}
