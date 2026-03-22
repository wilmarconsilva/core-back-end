using Application.DataTransferObjects.Entities;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers
{
    [ApiController]
    public abstract class CrudController<TEntitySaveDto> : Controller where TEntitySaveDto : EntitySaveDto
    {
        private readonly ICrudService<TEntitySaveDto> _service;

        protected CrudController(ICrudService<TEntitySaveDto> service, ILogger logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntitySaveDto>> GetById(long id, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<TEntitySaveDto>(async () =>
            {
                TEntitySaveDto dto = await _service.GetByIdAsync(id, cancellationToken);
                return Ok(dto);
            }, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<TEntitySaveDto>> Create([FromBody] TEntitySaveDto dto, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<TEntitySaveDto>(async () =>
            {
                Entity entity = (await _service.CreateAsync([dto], cancellationToken)).First();
                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
            }, cancellationToken);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TEntitySaveDto>> Update(long id, [FromBody] TEntitySaveDto dto, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<TEntitySaveDto>(async () =>
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
        public async Task<ActionResult<List<TListing>>> GetAll<TListing, TFilter>(TFilter filter, CancellationToken cancellationToken)
            where TListing : EntityListing
            where TFilter : EntityListingFilter
        {
            return await ProcessRequestAsync<List<TListing>>(async () =>
            {
                List<TListing> dto = await _service.GetAllAsync<TListing, TFilter>(filter, cancellationToken);
                return Ok(dto);
            }, cancellationToken);
        }

    }
}