using Domain.Territories.Localities.Entities;
using Domain.Territories.Localities.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;
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
