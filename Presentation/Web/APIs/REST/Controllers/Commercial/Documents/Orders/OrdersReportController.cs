using Application.DataTransferObjects.Commercial.Documents.Orders.Reports;
using Application.DataTransferObjects.Listings.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Commercial.Documents.Orders
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersReportController : Controller
    {
        private readonly IOrderReportQuery _query;

        public OrdersReportController(IOrderReportQuery query, ILogger<OrdersReportController> logger, IUnitOfWork unitOfWork)
            : base(logger, unitOfWork)
        {
            _query = query;
        }

        [HttpGet]
        public async Task<ActionResult<OrderReportListing>> Get([FromQuery] OrderReportFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<OrderReportListing>(async () =>
            {
                OrderReportListing listing = await _query.Get(filter, cancellationToken);
                return Ok(listing);
            }, cancellationToken);
        }
    }
}
