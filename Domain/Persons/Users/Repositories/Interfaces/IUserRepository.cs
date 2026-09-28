using Domain.Persons.Users.Entities;
using Domain.Shared.Repositories.Interfaces;

namespace Domain.Persons.Users.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryCrud<User>
    {
        Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
