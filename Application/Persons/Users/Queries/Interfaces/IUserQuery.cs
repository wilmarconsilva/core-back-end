using Application.Persons.Users.DataTransferObjects;

namespace Application.Persons.Users.Queries.Interfaces
{
    public interface IUserQuery
    {
        Task<CurrentUserDto?> GetCurrentAsync(long userId, CancellationToken cancellationToken);
    }
}
