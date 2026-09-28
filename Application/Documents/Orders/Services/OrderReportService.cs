using Application.DataTransferObjects.Commercial.Documents.Reports;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using Application.Documents.DataTransferObjects.Orders.Reports;
using Application.Documents.Orders.Reports;
using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Reports;

namespace Application.Documents.Orders.Services
{
    public class OrderReportService : IOrderReportService
    {
        private readonly IOrderReportQuery _query;

        public OrderReportService(IOrderReportQuery orderReportQuery)
        {
            _query = orderReportQuery;
        }

        public async Task<OrderReportListing> Get(OrderReportFilter filter, CancellationToken cancellationToken)
        {
            List<OrderReport> reports = await _query.GetAsync(filter, cancellationToken);

            return new OrderReportListing
            {
                Registers = reports,
                Summary = new OrderReportSummary
                {
                    ValueTotal = reports.Sum(r => r.ValueTotal),
                }
            };
        }
    }
}
