using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Queries.Commercial.Documents.Orders;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Commercial.Documents
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
        public async Task<ActionResult<IEnumerable<OrderReport>>> Get([FromQuery] DocumentReportFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<IEnumerable<OrderReport>>(async () =>
            {
                IEnumerable<OrderReport> reports = await _query.Get(filter, cancellationToken);
                return Ok(reports);
            }, cancellationToken);
        }
    }
}
