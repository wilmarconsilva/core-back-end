using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.Interfaces.Services.Commercial.Documents;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Commercial.Documents
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetsController : CrudController<BudgetSaveDto>
    {
        public BudgetsController(IBudgetService service, ILogger<BudgetsController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork) { }
    }
}