using Domain.Entities.Security;

namespace Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepositoryCrud<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);

        Task<List<RefreshToken>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
