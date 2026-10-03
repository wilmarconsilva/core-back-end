using Application.Persons.Users.DataTransferObjects;

namespace Application.Persons.Users.Services.Interfaces
{
    public interface IUserService
    {
        Task<CurrentUserDto> GetCurrentAsync(CancellationToken cancellationToken);
    }
}
