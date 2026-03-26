using Application.DataTransferObjects.Security;
using Application.DataTransferObjects.Security.Authentications;

namespace Application.Interfaces.Services.Security
{
    public interface IAuthenticationService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);

        Task<ResponseDto> RefreshAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken);

        Task RevokeAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken);
    }
}
