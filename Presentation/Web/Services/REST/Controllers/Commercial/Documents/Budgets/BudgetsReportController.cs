using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Reports;
using Application.Interfaces.Services.Commercial.Documents.Budgets;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Commercial.Documents.Budgets
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetsReportController : Controller
    {
        private readonly IBudgetReportService _service;

        public BudgetsReportController(IBudgetReportService service, ILogger<BudgetsReportController> logger, IUnitOfWork unitOfWork)
            : base(logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<BudgetReportListing>> Get([FromQuery] BudgetReportFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<BudgetReportListing>(async () =>
            {
                BudgetReportListing listing = await _service.Get(filter, cancellationToken);
                return Ok(listing);
            }, cancellationToken);
        }
    }
}
