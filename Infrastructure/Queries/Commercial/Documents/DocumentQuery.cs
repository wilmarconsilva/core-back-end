using Application.DataTransferObjects.Commercial.Documents.Listings;
using Application.DataTransferObjects.Entities;
using Application.Interfaces.Queries;
using AutoMapper;
using Domain.Entities.Commercial.Documents;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries.Commercial.Documents
{
    public abstract class DocumentQuery<TEntity, TEntitySaveDto, TDocumentProduct, TListing, TFilter> : SaveDtoQuery<TEntity, TEntitySaveDto>, IEntityListingQuery<TListing, TFilter>
        where TEntity : Document<TDocumentProduct>
        where TEntitySaveDto : EntitySaveDto
        where TDocumentProduct : DocumentProduct
        where TListing : DocumentListing, new()
        where TFilter : DocumentListingFilter
    {
        protected DocumentQuery(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<TListing>> GetAll(TFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = _query;

            if (filter.Id.HasValue)
                query = query.Where(document => document.Id == filter.Id.Value);

            if (filter.CustomerId.HasValue)
                query = query.Where(document => document.CustomerId == filter.CustomerId.Value);

            if (filter.CreationDateTimeStart.HasValue)
                query = query.Where(document => document.CreationDateTime >= filter.CreationDateTimeStart.Value);

            if (filter.CreationDateTimeEnd.HasValue)
                query = query.Where(document => document.CreationDateTime <= filter.CreationDateTimeEnd.Value);

            return await ApplyPaging(query, filter)
                .Select(document => new TListing
                {
                    Id = document.Id,
                    CustormerName = document.Customer!.Name,
                    CustomerCityState = document.Customer!.Addresses
                            .OrderBy(address => address.Id)
                            .Select(address => address.Locality!.Description + (address.Locality.State == null ? string.Empty : $" - {address.Locality.State.Acronym}"))
                            .FirstOrDefault() ?? string.Empty,
                    ValueTotal = document.ValueTotal,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
