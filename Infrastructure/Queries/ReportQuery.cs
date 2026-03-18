using Application.Interfaces.Queries;
using Domain.Entities;
using Infrastructure.Persistece;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public abstract class ReportQuery<TEntity, TReport, TFilter> : AppDbContextAcessor, IReportQuery<TReport, TFilter>
        where TEntity : Entity
    {
        protected readonly IQueryable<TEntity> _query;

        protected ReportQuery(AppDbContext context) : base(context)
        {
            _query = _context.Set<TEntity>().AsNoTracking();
        }

        public abstract Task<IEnumerable<TReport>> Get(TFilter filter, CancellationToken cancellationToken);
    }
}
