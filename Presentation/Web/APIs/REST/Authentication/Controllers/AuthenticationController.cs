using Application.Authentication.DataTransferObjects;
using Application.Authentication.Services.Interfaces;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace REST.Authentication.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : Shared.Controllers.Controller
    {
        private const string RefreshTokenCookieName = "refreshToken";
        private const string RefreshTokenCookiePath = "/authentication";

        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AccessTokenDto>> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<AccessTokenDto>(async () =>
            {
                ResponseDto response = await _authenticationService.LoginAsync(request, cancellationToken);
                AppendRefreshTokenCookie(response.RefreshTokenDto);
                return Ok(response.AccessTokenDto);
            }, cancellationToken);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<AccessTokenDto>> Refresh(CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<AccessTokenDto>(async () =>
            {
                try
                {
                    ResponseDto response = await _authenticationService.RefreshAsync(new RefreshTokenRequestDto { RefreshToken = GetRefreshTokenFromCookie() }, cancellationToken);
                    AppendRefreshTokenCookie(response.RefreshTokenDto);
                    return Ok(response.AccessTokenDto);
                }
                catch (UnauthorizedAccessException)
                {
                    DeleteRefreshTokenCookie();
                    throw;
                }
            }, cancellationToken);
        }

        [AllowAnonymous]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync(async () =>
            {
                if (Request.Cookies.TryGetValue(RefreshTokenCookieName, out string? refreshToken) && !string.IsNullOrWhiteSpace(refreshToken))
                    await _authenticationService.RevokeAsync(new RefreshTokenRequestDto { RefreshToken = refreshToken }, cancellationToken);

                DeleteRefreshTokenCookie();
                return NoContent();
            }, cancellationToken);
        }

        private string GetRefreshTokenFromCookie()
        {
            return Request.Cookies.TryGetValue(RefreshTokenCookieName, out string? refreshToken) && !string.IsNullOrWhiteSpace(refreshToken)
                ? refreshToken
                : throw new UnauthorizedAccessException("Refresh Token não informado.");
        }

        private CookieOptions GetCookieOptions(DateTime? refreshTokenExpirationDateTime)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Path = RefreshTokenCookiePath,
                SameSite = Request.HttpContext.Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
                Secure = Request.HttpContext.Request.IsHttps,
                Expires = refreshTokenExpirationDateTime != null ? new DateTimeOffset(refreshTokenExpirationDateTime.Value) : null
            };
        }

        private void AppendRefreshTokenCookie(RefreshTokenDto refreshToken)
        {
            Response.Cookies.Append(RefreshTokenCookieName, refreshToken.RefreshToken, GetCookieOptions(refreshToken.RefreshTokenExpirationDateTime));
        }

        private void DeleteRefreshTokenCookie()
        {
            Response.Cookies.Delete(RefreshTokenCookieName, GetCookieOptions(null));
        }

    }
}
