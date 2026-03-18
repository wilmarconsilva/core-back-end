namespace Application.DataTransferObjects.Commercial.Documents.Orders
{
    public class OrderReport : DocumentReport<OrderProductReport>
    {
        public long? BudgetId { get; set; }
    }
}
