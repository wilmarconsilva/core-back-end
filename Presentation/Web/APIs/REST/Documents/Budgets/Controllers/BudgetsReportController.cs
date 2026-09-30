using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Reports;
using Application.Documents.Budgets.Services.Interfaces;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace REST.Documents.Budgets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetsReportController : Shared.Controllers.Controller
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
