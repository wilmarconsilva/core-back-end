using Application.Documents.Orders.DataTransferObjects.Reports;
using Application.Documents.Orders.Queries;
using Infrastructure.Documents.Queries;
using Infrastructure.Shared.Persistance;

namespace Infrastructure.Documents.Orders.Queries
{
    public class OrderReportQuery : DocumentReportQuery<OrderReport, OrderReportFilter>, IOrderReportQuery
    {
        public OrderReportQuery(AppDbContext context) : base(context)
        {
        }

        protected override string GetTableName() => "orders";
    }
}