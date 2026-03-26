using Application.DataTransferObjects.Security;
using Application.DataTransferObjects.Security.Authentications;
using Application.Interfaces.Services.Security;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<ResponseDto>(async () =>
            {
                ResponseDto response = await _authenticationService.LoginAsync(request, cancellationToken);
                return Ok(response);
            }, cancellationToken);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<ResponseDto>> Refresh([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<ResponseDto>(async () =>
            {
                ResponseDto response = await _authenticationService.RefreshAsync(request, cancellationToken);
                return Ok(response);
            }, cancellationToken);
        }

        [AllowAnonymous]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync(async () =>
            {
                await _authenticationService.RevokeAsync(request, cancellationToken);
                return NoContent();
            }, cancellationToken);
        }
    }
}
