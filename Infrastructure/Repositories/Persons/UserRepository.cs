using Domain.Persons.Users.Entities;
using Domain.Persons.Users.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Persons
{
    public class UserRepository(AppDbContext context) : RepositoryCrud<User>(context), IUserRepository
    {
        public Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return _entityDbSet.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return _entityDbSet.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower(), cancellationToken);
        }
    }
}
