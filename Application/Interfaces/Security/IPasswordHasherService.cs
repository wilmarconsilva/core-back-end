using Domain.Entities.Persons;

namespace Application.Interfaces.Security
{
    public interface IPasswordHasherService
    {
        string HashPassword(User user, string password);

        bool VerifyPassword(User user, string passwordHash, string providedPassword);
    }
}
