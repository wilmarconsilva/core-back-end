using Domain.Entities.Persons;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;
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
