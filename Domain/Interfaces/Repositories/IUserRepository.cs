using Domain.Entities.Persons;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryCrud<User>
    {
        Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
