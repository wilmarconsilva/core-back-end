using Application.Documents.Orders.DataTransferObjects;
using Domain.Documents.Orders.ValueObjects;

namespace Application.Mappings.Commercial.Documents.Orders
{
    public class OrderPaymentProfile : CrudProfile<OrderPaymentSaveDto, OrderPayment>
    {
    }
}
