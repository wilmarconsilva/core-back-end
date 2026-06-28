using Domain.Entities.Territories;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Territories
{
    public class LocalityRepository(AppDbContext context) : RepositoryCrud<Locality>(context), ILocalityRepository
    {
        public async Task<long> GetQuantityAsync(CancellationToken cancellationToken)
        {
            return await _entityDbSet.LongCountAsync(cancellationToken);
        }
    }
}
