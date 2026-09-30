using Domain.Shared.Repositories.Interfaces;
using Domain.Territories.Localities.Entities;

namespace Domain.Territories.Localities.Repositories.Interfaces
{
    public interface ILocalityRepository : IRepositoryCrud<Locality>
    {
        Task<long> GetQuantityAsync(CancellationToken cancellationToken);
    }
}
