using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;

namespace Application.Interfaces.Queries
{
    /// <summary>Consulta de listagem + detalhe (GET por id). Persistência usa SaveDto separado.</summary>
    public interface IEntityCrudQuery<TDetailDto, TListing, TFilter> : IEntityListingQuery<TListing, TFilter>
        where TDetailDto : class
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        Task<TDetailDto?> GetDetailAsync(long id, CancellationToken cancellationToken);
    }
}
