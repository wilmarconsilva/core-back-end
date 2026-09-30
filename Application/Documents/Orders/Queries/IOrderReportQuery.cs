using Application.Documents.Orders.DataTransferObjects.Reports;
using Application.Shared.Queries.Interfaces;

namespace Application.Documents.Orders.Queries
{
    public interface IOrderReportQuery : IReportQuery<OrderReport, OrderReportFilter>
    {
    }
}
