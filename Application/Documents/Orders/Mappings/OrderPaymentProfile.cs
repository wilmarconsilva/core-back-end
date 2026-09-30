using Application.Documents.Orders.DataTransferObjects;
using Application.Shared.Mappings;
using Domain.Documents.Orders.ValueObjects;

namespace Application.Documents.Orders.Mappings
{
    public class OrderPaymentProfile : CrudProfile<OrderPaymentSaveDto, OrderPayment>
    {
    }
}
