using Application.Interfaces.Services;
using Application.Shared.DataTransferObjects.Entities;
using Application.Shared.DataTransferObjects.Listings;
using Domain.Shared.Entities;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace REST.Shared.Controllers
{
    [ApiController]
    public abstract class CrudController<TSaveDto, TDetailDto, TListing, TFilter> : Controller
        where TSaveDto : EntitySaveDto
        where TDetailDto : class
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        private readonly ICrudService<TSaveDto, TDetailDto, TListing, TFilter> _service;

        protected CrudController(ICrudService<TSaveDto, TDetailDto, TListing, TFilter> service, ILogger logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TDetailDto>> GetById(long id, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<TDetailDto>(async () =>
            {
                TDetailDto dto = await _service.GetByIdAsync(id, cancellationToken);
                return Ok(dto);
            }, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<TSaveDto>> Create([FromBody] TSaveDto dto, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<TSaveDto>(async () =>
            {
                Entity entity = (await _service.CreateAsync([dto], cancellationToken)).First();
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
            }, cancellationToken);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TSaveDto>> Update(long id, [FromBody] TSaveDto dto, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<TSaveDto>(async () =>
            {
                await _service.UpdateAsync(id, dto, cancellationToken);
                return Ok(dto);
            }, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync(async () =>
            {
                await _service.DeleteAsync(id, cancellationToken);
                return NoContent();
            }, cancellationToken);
        }

        [HttpGet]
        public abstract Task<ActionResult<List<TListing>>> GetAll([FromQuery] TFilter filter, CancellationToken cancellationToken);
    }
}
