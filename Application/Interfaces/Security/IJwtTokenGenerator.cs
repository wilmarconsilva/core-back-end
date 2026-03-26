using Application.Models.Security;
using Domain.Entities.Persons;

namespace Application.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        AccessTokenResult Generate(User user);

        DateTime GetRefreshTokenExpirationDateTime();
    }
}
