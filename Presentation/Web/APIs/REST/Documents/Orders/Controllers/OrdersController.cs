using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.DataTransferObjects.Listings;
using Application.Interfaces.Services.Commercial.Documents.Orders;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;
using REST.Shared.Controllers;

namespace REST.Documents.Orders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : CrudController<OrderSaveDto, OrderDetailDto, OrderListing, OrderListingFilter>
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service, ILogger<OrdersController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet]
        public override async Task<ActionResult<List<OrderListing>>> GetAll([FromQuery] OrderListingFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<List<OrderListing>>(async () =>
            {
                List<OrderListing> listings = await _service.GetAllAsync(filter, cancellationToken);
                return Ok(listings);
            }, cancellationToken);
        }
    }
}
