using Application.Documents.Orders.DataTransferObjects.Reports;

namespace Application.Interfaces.Queries.Commercial.Documents.Orders
{
    public interface IOrderReportQuery : IReportQuery<OrderReport, OrderReportFilter>
    {
    }
}
