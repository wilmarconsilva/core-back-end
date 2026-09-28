using Application.Documents.DataTransferObjects;

namespace Application.Documents.Orders.DataTransferObjects
{
    public class OrderSaveDto : DocumentSaveDto<OrderProductSaveDto>
    {
        public required OrderPaymentSaveDto Payment { get; set; }
    }
}
