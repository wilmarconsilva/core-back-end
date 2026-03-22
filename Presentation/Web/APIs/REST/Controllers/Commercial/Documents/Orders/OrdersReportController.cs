using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Commercial.Documents.Orders.Reports;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Commercial.Documents.Orders
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersReportController : Controller
    {
        private readonly IOrderReportService _service;

        public OrdersReportController(IOrderReportService service, ILogger<OrdersReportController> logger, IUnitOfWork unitOfWork)
            : base(logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<OrderReportListing>> Get([FromQuery] OrderReportFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<OrderReportListing>(async () =>
            {
                OrderReportListing listing = await _service.Get(filter, cancellationToken);
                return Ok(listing);
            }, cancellationToken);
        }
    }
}
