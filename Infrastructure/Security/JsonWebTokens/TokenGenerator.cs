using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Security;
using Application.Models.Security;
using Domain.Entities.Persons;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace Infrastructure.Security.JsonWebTokens
{
    public class TokenGenerator : IJwtTokenGenerator
    {
        private readonly Options _jwtOptions;

        public TokenGenerator(IOptions<Options> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public AccessTokenResult Generate(User user)
        {
            DateTime expirationDateTime = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);
            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
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

            return new AccessTokenResult
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
