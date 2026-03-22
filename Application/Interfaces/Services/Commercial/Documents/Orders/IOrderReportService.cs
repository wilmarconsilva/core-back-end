using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Reports;

namespace Application.Interfaces.Services.Commercial.Documents.Orders
{
    public interface IOrderReportService : IReportService<OrderReportListing, OrderReportFilter>
    {

    }
}
