using Application.Documents.DataTransferObjects;

namespace Application.Documents.Orders.DataTransferObjects
{
    public class OrderDetailDto : DocumentDetailDto
    {
        public long? BudgetId { get; set; }
    }
}
