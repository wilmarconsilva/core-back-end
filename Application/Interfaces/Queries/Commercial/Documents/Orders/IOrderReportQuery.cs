using Application.DataTransferObjects.Commercial.Documents.Orders.Reports;

namespace Application.Interfaces.Queries.Commercial.Documents.Orders
{
    public interface IOrderReportQuery : IReportQuery<OrderReport, OrderReportFilter>
    {
    }
}
