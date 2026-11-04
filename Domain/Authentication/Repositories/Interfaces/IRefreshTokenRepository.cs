using Domain.Authentication.Entities;
using Domain.Shared.Repositories.Interfaces;

namespace Domain.Authentication.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IRepositoryCrud<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);

        Task<List<RefreshToken>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
