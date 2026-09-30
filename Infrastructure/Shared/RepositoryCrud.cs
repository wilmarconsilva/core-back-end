using System.Linq.Expressions;
using Domain.Shared.Entities;
using Domain.Shared.Repositories.Interfaces;
using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Shared
{
    public abstract class RepositoryCrud<TEntity> : AppDbContextAcessor, IRepositoryCrud<TEntity> where TEntity : Entity
    {
        protected readonly DbSet<TEntity> _entityDbSet;

        protected RepositoryCrud(AppDbContext context) : base(context)
        {
            _entityDbSet = _context.Set<TEntity>();
        }

        #region Public methods

        public async Task SaveAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
           await _entityDbSet.AddRangeAsync(entities.Where(e => e.Id == 0), cancellationToken);

           _entityDbSet.UpdateRange(entities.Where(e => e.Id > 0));
        }

        public Task<List<TEntity>> GetByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = _entityDbSet.Where(e => ids.Contains(e.Id));

            return ApplyIncludesForGetByIds(query).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<long>> GetIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            return await _entityDbSet.Where(e => ids.Contains(e.Id)).Select(e => e.Id).ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken)
        {
            return (await GetIdsAsync([id], cancellationToken)).Count > 0;
        }

        public Task<int> DeleteAsync(IEnumerable<long> ids, CancellationToken cancellationToken)
        {
            string? tableName = _context.Model.FindEntityType(typeof(TEntity))?.GetTableName();

            string columnName = GetColumnName(p => p.Id);

            string sql = $@"DELETE FROM ""{tableName}"" WHERE ""{columnName}"" = ANY(@ids);";

            Npgsql.NpgsqlParameter<long[]> parameter = new("ids", ids.ToArray());

            return _context.Database.ExecuteSqlRawAsync(sql, [parameter], cancellationToken);
        }

        #endregion Public methods

        #region Protected methods

        protected virtual IQueryable<TEntity> ApplyIncludesForGetByIds(IQueryable<TEntity> query)
        {
            return query;
        }

        #endregion Protected methods

        #region Private methods

        private string GetColumnName(Expression<Func<Entity, object>> propertyExpression)
        {
            string entityName = typeof(TEntity).Name;

            IEntityType? entityType = _context.Model.FindEntityType(typeof(TEntity)) ?? throw new KeyNotFoundException($"Entidade não encontrada para o tipo {entityName}");

            IProperty? property = entityType.FindProperty(propertyExpression.GetPropertyAccess().Name) ?? throw new KeyNotFoundException($"Propriedade não encontrada para o tipo {entityName}");

            return property.GetColumnName();
        }

        #endregion Private methods
    }
}