using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Application.Interfaces.Queries;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistece;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries
{
    public abstract class SaveDtoQuery<TEntity, TEntitySaveDto> : AppDbContextAcessor, IEntitySaveDtoQuery<TEntitySaveDto> where TEntity : Entity where TEntitySaveDto : EntitySaveDto
    {
        protected readonly IQueryable<TEntity> _query;
        protected readonly IMapper _mapper;

        protected SaveDtoQuery(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _query = _context.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntitySaveDto?> GetSaveDto(long id, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = _query.Where(c => c.Id == id);

            query = ApplyIncludes(query);

            TEntity? entity = await query.FirstOrDefaultAsync(cancellationToken);

            return entity == null ? null : _mapper.Map<TEntitySaveDto>(entity);
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