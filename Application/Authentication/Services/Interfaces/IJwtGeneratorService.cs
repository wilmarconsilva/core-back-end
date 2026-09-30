using Domain.Persons.Users.Entities;

namespace Application.Authentication.Services.Interfaces
{      
    public interface IJwtGeneratorService
    {
        AccessToken Generate(User user);

        DateTime GetRefreshTokenExpirationDateTime();
    }
}
