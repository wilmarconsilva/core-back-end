using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.Territories.Localities.Entities;
using Application.Territories.Localities.DataTransferObjects;
using Application.Territories.Localities.DataTransferObjects.Listings;
using Infrastructure.Shared.Persistance;
using Infrastructure.Shared;
using Infrastructure.Shared.Queries;
using Application.Territories.Localities.Queries.Interfaces;

namespace Infrastructure.Territories.Localities.Queries
{
    public class LocalityQuery : DetailDtoQuery<Locality, LocalityDetailDto>, ILocalityQuery
    {
        public LocalityQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override IQueryable<Locality> ApplyIncludes(IQueryable<Locality> query)
        {
            return query.Include(l => l.State);
        }

        public Task<List<LocalityListing>> GetAll(LocalityListingFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<Locality> query = _query;

            if (filter.Id.HasValue)
                query = query.Where(l => l.Id == filter.Id.Value);

            if (filter.Description != null)
                query = query.WhereContains(l => l.Description, filter.Description);

            if (filter.StateId.HasValue)
                query = query.Where(l => l.StateId == filter.StateId.Value);

            return ApplyPaging(query, filter)
                .Select(l => new LocalityListing
                {
                    Id = l.Id,
                    Description = l.Description,
                    StateId = l.StateId,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
