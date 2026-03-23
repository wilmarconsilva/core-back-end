using Application.DataTransferObjects.Commercial.Documents.Reports;
using Application.DataTransferObjects.Commercial.Documents.Orders.Reports;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders;

namespace Application.Services.Commercial.Documents.Orders
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
