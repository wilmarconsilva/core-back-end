using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Domain.Entities.Commercial.Documents.Orders;
using Infrastructure.Persistece;
using Infrastructure.Queries.Commercial.Documents;

namespace Infrastructure.Queries.Commercial.Documents.Orders
{
    public class OrderReportQuery : DocumentReportQuery<Order, OrderProduct, OrderReport, OrderProductReport>, IOrderReportQuery
    {
        public OrderReportQuery(AppDbContext context) : base(context)
        {
        }

        protected override void MapExtraProperties(OrderReport report, Order order)
        {
            report.BudgetId = order.BudgetId;
        }
    }
}
