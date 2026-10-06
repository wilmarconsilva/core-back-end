using Application.Persons.Users.DataTransferObjects;
using Application.Persons.Users.Services.Interfaces;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace REST.Persons.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Shared.Controllers.Controller
    {
        private readonly IUserService _service;

        public UsersController(IUserService service, ILogger<UsersController> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet("current")]
        public async Task<ActionResult<CurrentUserDto>> GetCurrent(CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<CurrentUserDto>(async () =>
            {
                CurrentUserDto user = await _service.GetCurrentAsync(cancellationToken);
                return Ok(user);
            }, cancellationToken);
        }
    }
}
