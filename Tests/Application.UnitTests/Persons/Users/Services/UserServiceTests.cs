using Application.Authentication.Interfaces;
using Application.Persons.Users.DataTransferObjects;
using Application.Persons.Users.Queries.Interfaces;
using Application.Persons.Users.Services;
using Domain.Shared.Exceptions;

namespace Application.UnitTests.Persons.Users.Services
{
    public class UserServiceTests
    {
        private readonly IUserQuery _query = Substitute.For<IUserQuery>();
        private readonly UserService _service;

        public UserServiceTests()
        {
            IHttpContextCurrentUser currentUser = Substitute.For<IHttpContextCurrentUser>();
            currentUser.UserId.Returns(3);

            _service = new UserService(_query, currentUser);
        }

        [Fact]
        public async Task GetCurrentAsync_UserNotFound_ThrowsNotFoundException()
        {
            _query.GetCurrentAsync(3, Arg.Any<CancellationToken>()).Returns((CurrentUserDto?)null);

            NotFoundException exception = await Should.ThrowAsync<NotFoundException>(() => _service.GetCurrentAsync(TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Usuário autenticado não encontrado.");
        }

        [Fact]
        public async Task GetCurrentAsync_UserFound_ReturnsCurrentUser()
        {
            CurrentUserDto user = new() { Name = "Maria" };
            _query.GetCurrentAsync(3, Arg.Any<CancellationToken>()).Returns(user);

            CurrentUserDto result = await _service.GetCurrentAsync(TestContext.Current.CancellationToken);

            result.ShouldBeSameAs(user);
        }
    }
}
