using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRepositoryCrud<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        Task SaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<IReadOnlyList<long>> GetIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken);
        
        Task<int> DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task<List<TEntity>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken);
    }
}