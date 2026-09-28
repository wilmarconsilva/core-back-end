using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Reports;

namespace Application.Interfaces.Services.Commercial.Documents.Orders
{
    public interface IOrderReportService : IReportService<OrderReportListing, OrderReportFilter>
    {

    }
}
