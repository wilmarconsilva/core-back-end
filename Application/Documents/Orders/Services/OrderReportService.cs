using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Reports;
using Application.Documents.Orders.Queries;
using Application.Documents.Orders.Services.Interfaces;

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
