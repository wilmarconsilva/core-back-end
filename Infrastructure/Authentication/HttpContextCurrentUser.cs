using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Authentication.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;

public sealed class HttpContextCurrentUser : IHttpContextCurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long UserId
    {
        get
        {
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            string? raw = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrWhiteSpace(raw) || !long.TryParse(raw, out long id))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            return id;
        }
    }
}
