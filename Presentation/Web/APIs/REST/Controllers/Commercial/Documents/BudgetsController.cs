using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Budgets.Listings;
using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;
using REST.Controllers.Commercial.Documents.Orders;

namespace REST.Controllers.Commercial.Documents
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

        [HttpPost("{id}/orders")]
        public async Task<ActionResult<OrderDetailDto>> CreateOrderByBudget([FromRoute] long id, [FromBody] OrderByBudgetDto orderByBudgetDto, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<OrderDetailDto>(async () =>
            {
                OrderDetailDto orderDetail = await _service.CreateOrderByBudget(id, orderByBudgetDto, cancellationToken);
                return CreatedAtAction(nameof(OrdersController.GetById), nameof(OrdersController).Replace("Controller", string.Empty), new { id = orderDetail.Id }, orderDetail);
            }, cancellationToken);
        }
    }
}