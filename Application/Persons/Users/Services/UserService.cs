using Application.Authentication.Interfaces;
using Application.Persons.Users.DataTransferObjects;
using Application.Persons.Users.Queries.Interfaces;
using Application.Persons.Users.Services.Interfaces;
using Domain.Shared.Exceptions;

namespace Application.Persons.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserQuery _query;
        private readonly IHttpContextCurrentUser _currentUser;

        public UserService(IUserQuery query, IHttpContextCurrentUser currentUser)
        {
            _query = query;
            _currentUser = currentUser;
        }

        public async Task<CurrentUserDto> GetCurrentAsync(CancellationToken cancellationToken)
        {
            return await _query.GetCurrentAsync(_currentUser.UserId, cancellationToken)
                ?? throw new NotFoundException("Usuário autenticado não encontrado.");
        }
    }
}
