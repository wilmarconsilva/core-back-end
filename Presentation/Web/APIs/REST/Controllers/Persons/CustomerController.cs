using Application.DataTransferObjects.Persons.Customers;
using Application.DataTransferObjects.Persons.Customers.Listings;
using Application.Interfaces.Services.Persons;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Persons
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : CrudController<CustomerSaveDto, CustomerListing, CustomerListingFilter>
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