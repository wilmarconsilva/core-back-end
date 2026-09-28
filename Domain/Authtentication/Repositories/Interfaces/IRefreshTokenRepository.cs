using Domain.Authtentication.Entities;
using Domain.Shared.Repositories.Interfaces;

namespace Domain.Authtentication.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IRepositoryCrud<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);

        Task<List<RefreshToken>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
