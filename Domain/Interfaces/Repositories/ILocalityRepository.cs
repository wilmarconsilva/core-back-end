using Domain.Entities.Territories;

namespace Domain.Interfaces.Repositories
{
    public interface ILocalityRepository : IRepositoryCrud<Locality>
    {
        Task<long> GetQuantityAsync(CancellationToken cancellationToken);
    }
}
