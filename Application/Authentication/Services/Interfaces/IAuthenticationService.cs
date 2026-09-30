using Application.Authentication.DataTransferObjects;

namespace Application.Authentication.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);

        Task<ResponseDto> RefreshAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken);

        Task RevokeAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken);
    }
}
