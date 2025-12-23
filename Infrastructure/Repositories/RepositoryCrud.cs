using System.Linq.Expressions;
using Domain.Entities;
using Domain.Entities.Persons;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Repositories
{
    public abstract class RepositoryCrud<TEntity> : IRepositoryCrud<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext _context;

        protected RepositoryCrud(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            return _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return AddAsync([entity], cancellationToken);
        }

        public ValueTask<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return _context.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public Task<List<long>> GetExistingIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            return _context.Set<TEntity>().Where(e => ids.Contains(e.Id)).Select(e => e.Id).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken)
        {
            return (await GetExistingIdsAsync([id], cancellationToken)).Count > 0;
        }

        public Task UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return UpdateAsync([entity], cancellationToken);
        }

        public Task<int> DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            string? tableName = _context.Model.FindEntityType(typeof(TEntity))?.GetTableName();

            if (string.IsNullOrEmpty(tableName))
                throw new KeyNotFoundException($"Tabela não encontrada para a entidade {typeof(TEntity).Name}");

            return _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM {tableName} WHERE c{GetColumnName(p => p.Id)} in ({string.Join(",", ids)});", cancellationToken);
        }

        #region Private methods

        private string GetColumnName(Expression<Func<Entity, object>> propertyExpression)
        {
        IEntityType? entityType = _context.Model.FindEntityType(typeof(Entity)) ?? throw new KeyNotFoundException($"Entidade não encontrada para o tipo {typeof(Entity).Name}");

        IProperty? property = entityType.FindProperty(propertyExpression.GetPropertyAccess().Name) ?? throw new KeyNotFoundException($"Propriedade não encontrada para o tipo {typeof(Entity).Name}");

        return property.GetColumnName();
        }

        #endregion Private methods
    }
}