using Application.Documents.DataTransferObjects;

namespace Application.Documents.Orders.DataTransferObjects
{
    public class OrderDetailDto : DocumentDetailDto
    {
        public required OrderPaymentSaveDto Payment { get; set; }

        public long? BudgetId { get; set; }
    }
}
