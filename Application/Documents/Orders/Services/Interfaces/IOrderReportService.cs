using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Reports;
using Application.Shared.Services.Interfaces;

namespace Application.Documents.Orders.Services.Interfaces
{
    public interface IOrderReportService : IReportService<OrderReportListing, OrderReportFilter>
    {

    }
}
