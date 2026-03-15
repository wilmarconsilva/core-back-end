using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.Interfaces.Services.Commercial.Documents;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Commercial.Documents
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : CrudController<OrderSaveDto>
    {
        public OrdersController(IOrderService service, ILogger<OrdersController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork) { }
    }
}
