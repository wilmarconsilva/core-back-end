using Application.Authentication.Services.Interfaces;
using Domain.Persons.Users.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string passwordHash, string providedPassword)
        {
            try
            {
                PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, passwordHash, providedPassword);
                return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
