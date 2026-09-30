using Domain.Persons.Users.Entities;

namespace Application.Authentication.Services.Interfaces
{
    public interface IPasswordHasherService
    {
        string HashPassword(User user, string password);

        bool VerifyPassword(User user, string passwordHash, string providedPassword);
    }
}
