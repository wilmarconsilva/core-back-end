using Application.DataTransferObjects.Territories;
using Application.DataTransferObjects.Territories.Listings;
using Application.Interfaces.Services.Territories;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Territories
{
    [ApiController]
    [Route("[controller]")]
    public class LocalitiesController : CrudController<LocalitySaveDto, LocalityListing, LocalityListingFilter>
    {
        private readonly ILocalityService _service;

        public LocalitiesController(ILocalityService service, ILogger<LocalitiesController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet]
        public override async Task<ActionResult<List<LocalityListing>>> GetAll([FromQuery] LocalityListingFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<List<LocalityListing>>(async () =>
            {
                List<LocalityListing> listings = await _service.GetAllAsync(filter, cancellationToken);
                return Ok(listings);
            }, cancellationToken);
        }
    }
}
