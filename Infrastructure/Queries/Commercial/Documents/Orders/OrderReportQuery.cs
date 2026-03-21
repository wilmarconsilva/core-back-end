using Application.DataTransferObjects.Commercial.Documents.Orders.Reports;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Infrastructure.Persistece;

namespace Infrastructure.Queries.Commercial.Documents.Orders
{
    public class OrderReportQuery : DocumentReportQuery<OrderReport>, IOrderReportQuery
    {
        public OrderReportQuery(AppDbContext context) : base(context)
        {
        }

        protected override string GetTableName() => "orders";
    }
}