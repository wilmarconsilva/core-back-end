using Application.DataTransferObjects.Listings;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistece;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public abstract class DetailDtoQuery<TEntity, TDetailDto> : AppDbContextAcessor
        where TEntity : Entity
        where TDetailDto : class
    {
        protected readonly IQueryable<TEntity> _query;
        protected readonly IMapper _mapper;

        protected DetailDtoQuery(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _query = _context.Set<TEntity>().AsNoTracking();
        }

        public async Task<TDetailDto?> GetDetailAsync(long id, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = ApplyIncludes(_query.Where(e => e.Id == id));
            TEntity? entity = await query.FirstOrDefaultAsync(cancellationToken);
            return entity == null ? null : _mapper.Map<TDetailDto>(entity);
        }

        protected virtual IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query)
        {
            return query;
        }

        protected IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, ListingFilter filter)
        {
            IOrderedQueryable<TEntity> ordered = query.OrderByDescending(e => e.Id);

            return ordered
                .Skip(filter.RegistersStart)
                .Take(filter.RegistersEnd - filter.RegistersStart);
        }
    }
}
