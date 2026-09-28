using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.DataTransferObjects.Listings;
using Application.Documents.DataTransferObjects;
using Application.Interfaces.Services.Commercial.Documents;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;
using REST.Shared.Controllers;

namespace REST.Documents.Budgets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetsController : CrudController<BudgetSaveDto, DocumentDetailDto, BudgetListing, BudgetListingFilter>
    {
        private readonly IBudgetService _service;

        public BudgetsController(IBudgetService service, ILogger<BudgetsController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork) 
        {
            _service = service;
        }

        [HttpGet]
        public override async Task<ActionResult<List<BudgetListing>>> GetAll([FromQuery] BudgetListingFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<List<BudgetListing>>(async () =>
            {
                List<BudgetListing> listings = await _service.GetAllAsync(filter, cancellationToken);
                return Ok(listings);
            }, cancellationToken);
        }
    }
}