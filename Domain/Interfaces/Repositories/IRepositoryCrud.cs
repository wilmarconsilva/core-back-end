using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IRepositoryCrud<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task<IReadOnlyList<long>> GetIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken);

        Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken);

        Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
                
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        
        Task<int> DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken);
    }
}