using Application.Interfaces.Services.Persons;
using Application.Persons.Customers.DataTransferObjects;
using Application.Persons.Customers.DataTransferObjects.Listings;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;
using REST.Shared.Controllers;

namespace REST.Persons.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : CrudController<CustomerSaveDto, CustomerDetailDto, CustomerListing, CustomerListingFilter>
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service, ILogger<CustomersController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet]
        public override async Task<ActionResult<List<CustomerListing>>> GetAll([FromQuery] CustomerListingFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<List<CustomerListing>>(async () =>
            {
                List<CustomerListing> listings = await _service.GetAllAsync(filter, cancellationToken);
                return Ok(listings);
            }, cancellationToken);
        }
    }
}