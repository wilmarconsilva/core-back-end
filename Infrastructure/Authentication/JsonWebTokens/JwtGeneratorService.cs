using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Domain.Persons.Users.Entities;
using Application.Authentication;
using Application.Authentication.Services.Interfaces;

namespace Infrastructure.Authentication.JsonWebTokens
{
    public class JwtGeneratorService : IJwtGeneratorService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtGeneratorService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public AccessToken Generate(User user)
        {
            DateTime expirationDateTime = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);
            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey!)),
                SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.UniqueName, user.Name),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name)
            ];

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expirationDateTime,
                signingCredentials: signingCredentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new AccessToken
            {
                Token = token,
                ExpirationDateTime = expirationDateTime,
                UserName = user.Name
            };
        }

        public DateTime GetRefreshTokenExpirationDateTime()
        {
            return DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
        }
    }
}
