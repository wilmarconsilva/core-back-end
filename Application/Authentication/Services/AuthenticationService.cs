using System.Security.Cryptography;
using System.Text;
using Application.Authentication;
using Application.Authentication.DataTransferObjects;
using Application.Authentication.Services.Interfaces;
using Domain.Authentication.Entities;
using Domain.Authentication.Repositories.Interfaces;
using Domain.Persons.Users.Entities;
using Domain.Persons.Users.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;

namespace Application.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string InvalidCredentialsMessage = "Credenciais inválidas.";

        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtGeneratorService _jwtTokenGenerator;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtGeneratorService jwtTokenGenerator,
            IPasswordHasherService passwordHasherService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasherService = passwordHasherService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
                ?? throw new UnauthorizedAccessException(InvalidCredentialsMessage);

            bool shouldUpgradeLegacyPassword = false;
            if (!_passwordHasherService.VerifyPassword(user, user.PasswordHash, request.Password))
            {
                if (!string.Equals(user.PasswordHash, request.Password, StringComparison.Ordinal))
                    throw new UnauthorizedAccessException(InvalidCredentialsMessage);

                user.PasswordHash = _passwordHasherService.HashPassword(user, request.Password);
                shouldUpgradeLegacyPassword = true;
            }

            return await CreateTokensAsync(user, shouldUpgradeLegacyPassword, cancellationToken);
        }

        public async Task<ResponseDto> RefreshAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            RefreshToken currentRefreshToken = await ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);

            User user = await _userRepository.GetByIdAsync(currentRefreshToken.UserId, cancellationToken)
                ?? throw new UnauthorizedAccessException("Usuário não encontrado para o Refresh Token informado.");

            return await RotateRefreshTokenAsync(user, currentRefreshToken, cancellationToken);
        }

        public async Task RevokeAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken)
        {
            string refreshTokenHash = ComputeTokenHash(request.RefreshToken);
            RefreshToken? refreshToken = await _refreshTokenRepository.GetByTokenHashAsync(refreshTokenHash, cancellationToken);

            if (refreshToken == null || refreshToken.IsRevoked)
                return;

            refreshToken.Revoke();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _refreshTokenRepository.SaveAsync([refreshToken], cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task<ResponseDto> CreateTokensAsync(User user, bool persistUserChanges, CancellationToken cancellationToken)
        {
            AccessToken accessToken = _jwtTokenGenerator.Generate(user);
            string refreshTokenValue = CreateRefreshToken();
            string refreshTokenHash = ComputeTokenHash(refreshTokenValue);
            DateTime refreshTokenExpirationDateTime = _jwtTokenGenerator.GetRefreshTokenExpirationDateTime();
            RefreshToken refreshToken = new(user.Id, refreshTokenHash, refreshTokenExpirationDateTime);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (persistUserChanges)
                await _userRepository.SaveAsync([user], cancellationToken);

            await _refreshTokenRepository.SaveAsync([refreshToken], cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CreateAuthenticationResponse(accessToken, refreshTokenValue, refreshTokenExpirationDateTime);
        }

        private async Task<ResponseDto> RotateRefreshTokenAsync(User user, RefreshToken currentRefreshToken, CancellationToken cancellationToken)
        {
            AccessToken accessToken = _jwtTokenGenerator.Generate(user);
            string newRefreshTokenValue = CreateRefreshToken();
            string newRefreshTokenHash = ComputeTokenHash(newRefreshTokenValue);
            DateTime refreshTokenExpirationDateTime = _jwtTokenGenerator.GetRefreshTokenExpirationDateTime();
            RefreshToken newRefreshToken = new(user.Id, newRefreshTokenHash, refreshTokenExpirationDateTime);

            currentRefreshToken.Rotate(newRefreshTokenHash);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _refreshTokenRepository.SaveAsync([currentRefreshToken, newRefreshToken], cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return CreateAuthenticationResponse(accessToken, newRefreshTokenValue, refreshTokenExpirationDateTime);
        }

        private async Task<RefreshToken> ValidateRefreshTokenAsync(string refreshTokenValue, CancellationToken cancellationToken)
        {
            string refreshTokenHash = ComputeTokenHash(refreshTokenValue);
            RefreshToken refreshToken = await _refreshTokenRepository.GetByTokenHashAsync(refreshTokenHash, cancellationToken)
                ?? throw new UnauthorizedAccessException("Refresh Token não encontrado.");

            if (refreshToken.IsRevoked)
            {
                await RevokeActiveTokensAsync(refreshToken.UserId, cancellationToken);
                throw new UnauthorizedAccessException("Refresh Token já revogado.");
            }

            if (!refreshToken.IsExpired)
                return refreshToken;
            else
            {
                refreshToken.Revoke();

                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                await _refreshTokenRepository.SaveAsync([refreshToken], cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                throw new UnauthorizedAccessException("Refresh Token já expirado.");
            }
        }

        private async Task RevokeActiveTokensAsync(long userId, CancellationToken cancellationToken)
        {
            List<RefreshToken> activeTokens = await _refreshTokenRepository.GetActiveByUserIdAsync(userId, cancellationToken);

            if (!activeTokens.Any())
                return;

            foreach (RefreshToken activeToken in activeTokens)
                activeToken.Revoke();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _refreshTokenRepository.SaveAsync(activeTokens, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private static ResponseDto CreateAuthenticationResponse(AccessToken accessToken, string refreshToken, DateTime refreshTokenExpirationDateTime)
        {
            return new ResponseDto
            {
                AccessTokenDto = new AccessTokenDto { AccessToken = accessToken.Token, AccessTokenExpirationDateTime = accessToken.ExpirationDateTime, UserName = accessToken.UserName },
                RefreshTokenDto = new RefreshTokenDto { RefreshToken = refreshToken, RefreshTokenExpirationDateTime = refreshTokenExpirationDateTime }
            };
        }

        private static string CreateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private static string ComputeTokenHash(string token)
        {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(hash);
        }
    }
}
