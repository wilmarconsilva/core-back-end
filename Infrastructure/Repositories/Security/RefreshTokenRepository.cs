using Domain.Entities.Security;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Security
{
    public class RefreshTokenRepository(AppDbContext context) : RepositoryCrud<RefreshToken>(context), IRefreshTokenRepository
    {
        public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken)
        {
            return _entityDbSet.FirstOrDefaultAsync(refreshToken => refreshToken.TokenHash == tokenHash, cancellationToken);
        }

        public Task<List<RefreshToken>> GetActiveByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            DateTime utcNow = DateTime.UtcNow;

            return _entityDbSet
                .Where(refreshToken => refreshToken.UserId == userId && refreshToken.RevokedDateTime == null && refreshToken.ExpirationDateTime > utcNow)
                .ToListAsync(cancellationToken);
        }
    }
}
