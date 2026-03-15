using Application.DataTransferObjects;
using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents;
using Domain.Entities;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Commercial.Documents
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetsController : CrudController<BudgetSaveDto>
    {
        private readonly IBudgetService _service;

        public BudgetsController(IBudgetService service, ILogger<BudgetsController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork) 
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<OrderSaveDto>> CreateOrderByBudget(long id, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<OrderSaveDto>(async () =>
            {
                OrderSaveDto orderSaveDto = await _service.CreateOrderByBudget(id, cancellationToken);
                return CreatedAtAction(nameof(GetById), nameof(OrdersController), new { id = orderSaveDto.Id }, orderSaveDto);
            }, cancellationToken);
        }
    }
}