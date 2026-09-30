using Application.Documents.DataTransferObjects.Payments;
using Domain.Documents.ValueObjects.Payments.Enums;

namespace Application.Documents.Orders.DataTransferObjects
{
    public class OrderPaymentSaveDto : PaymentSaveDto
    {
        public required PaymentMethod Method { get; set; }
    }
}
