using System.Security.Cryptography;
using System.Text;
using Application.Authentication;
using Application.Authentication.DataTransferObjects;
using Application.Authentication.Services;
using Application.Authentication.Services.Interfaces;
using Domain.Authentication.Entities;
using Domain.Authentication.Repositories.Interfaces;
using Domain.Persons.Users.Entities;
using Domain.Persons.Users.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;

namespace Application.UnitTests.Authentication.Services
{
    public class AuthenticationServiceTests
    {
        private const string Password = "senha-segura";
        private const string RefreshTokenValue = "refresh-token";

        private static readonly DateTime RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        private readonly IJwtGeneratorService _jwtGenerator = Substitute.For<IJwtGeneratorService>();
        private readonly IPasswordHasherService _passwordHasher = Substitute.For<IPasswordHasherService>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly AuthenticationService _service;
        private readonly User _user = new("Maria", "maria@email.com", "hash") { Id = 1 };

        public AuthenticationServiceTests()
        {
            _service = new AuthenticationService(_userRepository, _refreshTokenRepository, _jwtGenerator, _passwordHasher, _unitOfWork);

            _jwtGenerator.Generate(Arg.Any<User>()).Returns(new AccessToken { Token = "jwt", ExpirationDateTime = DateTime.UtcNow.AddHours(1), UserName = "Maria" });
            _jwtGenerator.GetRefreshTokenExpirationDateTime().Returns(RefreshTokenExpiration);
            _userRepository.GetByEmailAsync("maria@email.com", Arg.Any<CancellationToken>()).Returns(_user);
            _userRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(_user);
        }

        private static string Hash(string token)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
        }

        private RefreshToken ArrangeStoredRefreshToken(DateTime? expirationDateTime = null)
        {
            RefreshToken token = new(_user.Id, Hash(RefreshTokenValue), expirationDateTime ?? DateTime.UtcNow.AddDays(1));
            _refreshTokenRepository.GetByTokenHashAsync(Hash(RefreshTokenValue), Arg.Any<CancellationToken>()).Returns(token);
            return token;
        }

        private static LoginRequestDto Login(string email = "maria@email.com", string password = Password)
        {
            return new LoginRequestDto { Email = email, Password = password };
        }

        private static RefreshTokenRequestDto RefreshRequest()
        {
            return new RefreshTokenRequestDto { RefreshToken = RefreshTokenValue };
        }

        [Fact]
        public async Task LoginAsync_UnknownEmail_ThrowsUnauthorizedAccessException()
        {
            UnauthorizedAccessException exception = await Should.ThrowAsync<UnauthorizedAccessException>(() => _service.LoginAsync(Login(email: "outro@email.com"), TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Credenciais inválidas.");
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ThrowsUnauthorizedAccessException()
        {
            _passwordHasher.VerifyPassword(_user, "hash", "errada").Returns(false);

            await Should.ThrowAsync<UnauthorizedAccessException>(() => _service.LoginAsync(Login(password: "errada"), TestContext.Current.CancellationToken));
            await _refreshTokenRepository.DidNotReceive().SaveAsync(Arg.Any<IEnumerable<RefreshToken>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task LoginAsync_ValidPassword_PersistsHashedRefreshToken()
        {
            _passwordHasher.VerifyPassword(_user, "hash", Password).Returns(true);

            ResponseDto response = await _service.LoginAsync(Login(), TestContext.Current.CancellationToken);

            response.AccessTokenDto.AccessToken.ShouldBe("jwt");
            response.AccessTokenDto.UserName.ShouldBe("Maria");
            response.RefreshTokenDto.RefreshTokenExpirationDateTime.ShouldBe(RefreshTokenExpiration);
            string expectedHash = Hash(response.RefreshTokenDto.RefreshToken);
            await _refreshTokenRepository.Received(1).SaveAsync(
                Arg.Is<IEnumerable<RefreshToken>>(t => t.Single().UserId == 1 && t.Single().TokenHash == expectedHash),
                Arg.Any<CancellationToken>());
            await _userRepository.DidNotReceive().SaveAsync(Arg.Any<IEnumerable<User>>(), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task LoginAsync_LegacyPlainTextPassword_UpgradesToHash()
        {
            User legacyUser = new("Maria", "legado@email.com", Password) { Id = 2 };
            _userRepository.GetByEmailAsync("legado@email.com", Arg.Any<CancellationToken>()).Returns(legacyUser);
            _passwordHasher.VerifyPassword(legacyUser, Password, Password).Returns(false);
            _passwordHasher.HashPassword(legacyUser, Password).Returns("novo-hash");

            await _service.LoginAsync(Login(email: "legado@email.com"), TestContext.Current.CancellationToken);

            legacyUser.PasswordHash.ShouldBe("novo-hash");
            await _userRepository.Received(1).SaveAsync(Arg.Is<IEnumerable<User>>(u => u.Single() == legacyUser), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RefreshAsync_UnknownToken_ThrowsUnauthorizedAccessException()
        {
            UnauthorizedAccessException exception = await Should.ThrowAsync<UnauthorizedAccessException>(() => _service.RefreshAsync(RefreshRequest(), TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Refresh Token não encontrado.");
        }

        [Fact]
        public async Task RefreshAsync_RevokedToken_RevokesAllActiveTokensAndThrows()
        {
            RefreshToken stored = ArrangeStoredRefreshToken();
            stored.Revoke();
            RefreshToken active = new(_user.Id, "outro-hash", DateTime.UtcNow.AddDays(1));
            _refreshTokenRepository.GetActiveByUserIdAsync(_user.Id, Arg.Any<CancellationToken>()).Returns([active]);

            UnauthorizedAccessException exception = await Should.ThrowAsync<UnauthorizedAccessException>(() => _service.RefreshAsync(RefreshRequest(), TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Refresh Token já revogado.");
            active.IsRevoked.ShouldBeTrue();
            await _refreshTokenRepository.Received(1).SaveAsync(Arg.Is<IEnumerable<RefreshToken>>(t => t.Single() == active), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RefreshAsync_ExpiredToken_RevokesAndThrows()
        {
            RefreshToken stored = ArrangeStoredRefreshToken(DateTime.UtcNow.AddSeconds(-1));

            UnauthorizedAccessException exception = await Should.ThrowAsync<UnauthorizedAccessException>(() => _service.RefreshAsync(RefreshRequest(), TestContext.Current.CancellationToken));

            exception.Message.ShouldBe("Refresh Token já expirado.");
            stored.IsRevoked.ShouldBeTrue();
            await _refreshTokenRepository.Received(1).SaveAsync(Arg.Is<IEnumerable<RefreshToken>>(t => t.Single() == stored), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RefreshAsync_UserNotFound_ThrowsUnauthorizedAccessException()
        {
            ArrangeStoredRefreshToken();
            _userRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns((User?)null);

            await Should.ThrowAsync<UnauthorizedAccessException>(() => _service.RefreshAsync(RefreshRequest(), TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task RefreshAsync_ValidToken_RotatesToken()
        {
            RefreshToken stored = ArrangeStoredRefreshToken();

            ResponseDto response = await _service.RefreshAsync(RefreshRequest(), TestContext.Current.CancellationToken);

            string newHash = Hash(response.RefreshTokenDto.RefreshToken);
            stored.IsRevoked.ShouldBeTrue();
            stored.ReplacedByTokenHash.ShouldBe(newHash);
            await _refreshTokenRepository.Received(1).SaveAsync(
                Arg.Is<IEnumerable<RefreshToken>>(t => t.Count() == 2 && t.First() == stored && t.Last().TokenHash == newHash),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RevokeAsync_UnknownToken_DoesNothing()
        {
            await _service.RevokeAsync(RefreshRequest(), TestContext.Current.CancellationToken);

            await _refreshTokenRepository.DidNotReceive().SaveAsync(Arg.Any<IEnumerable<RefreshToken>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RevokeAsync_AlreadyRevokedToken_DoesNothing()
        {
            ArrangeStoredRefreshToken().Revoke();

            await _service.RevokeAsync(RefreshRequest(), TestContext.Current.CancellationToken);

            await _refreshTokenRepository.DidNotReceive().SaveAsync(Arg.Any<IEnumerable<RefreshToken>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task RevokeAsync_ActiveToken_RevokesAndSaves()
        {
            RefreshToken stored = ArrangeStoredRefreshToken();

            await _service.RevokeAsync(RefreshRequest(), TestContext.Current.CancellationToken);

            stored.IsRevoked.ShouldBeTrue();
            await _refreshTokenRepository.Received(1).SaveAsync(Arg.Is<IEnumerable<RefreshToken>>(t => t.Single() == stored), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }
    }
}
