using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;
using Application.Territories.Localities.DataTransferObjects.Synchronizations;
using Application.Territories.Localities.Services.Interfaces;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;
using REST.Shared.Controllers;

namespace REST.Territories.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalitiesController : CrudController<LocalitySaveDto, LocalityDetailDto, LocalityListing, LocalityListingFilter>
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

        /// <summary>Localidades com a sigla do estado para o snapshot offline do front-end.</summary>
        [HttpGet("synchronization")]
        public async Task<ActionResult<List<LocalitySynchronizationDto>>> GetSynchronization([FromQuery] LocalityListingFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<List<LocalitySynchronizationDto>>(async () =>
            {
                List<LocalitySynchronizationDto> localities = await _service.GetSynchronizationAsync(filter, cancellationToken);
                return Ok(localities);
            }, cancellationToken);
        }

        [HttpGet("quantity")]
        public async Task<ActionResult<long>> GetQuantity(CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<long>(async () =>
            {
                long quantity = await _service.GetQuantityAsync(cancellationToken);
                return Ok(quantity);
            }, cancellationToken);
        }
    }
}
