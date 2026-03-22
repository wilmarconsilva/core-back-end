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
            return await _query
                .Where(document => filter.Id == 0 || document.Id == filter.Id)
                .OrderByDescending(document => document.Id)
                .Skip(filter.RegistersStart)
                .Take(filter.RegistersEnd)
                .Select(document => new TListing
                {
                    Id = document.Id,
                    CustormerName = document.Customer == null ? string.Empty : document.Customer.Name,
                    CustomerCityState = document.Customer == null
                        ? string.Empty
                        : document.Customer.Addresses
                            .OrderBy(address => address.Id)
                            .Select(address => address.Locality == null
                                ? string.Empty
                                : address.Locality.Description + (address.Locality.State == null ? string.Empty : $" - {address.Locality.State.Acronym}"))
                            .FirstOrDefault() ?? string.Empty,
                    ValueTotal = document.ValueTotal,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
