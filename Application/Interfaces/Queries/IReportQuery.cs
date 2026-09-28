using Application.Shared.DataTransferObjects.Listings;

namespace Application.Interfaces.Queries
{
    public interface IReportQuery<TReport, TListingFilter> where TReport : class where TListingFilter : ListingFilter

    {
        Task<List<TReport>> GetAsync(TListingFilter filter, CancellationToken cancellationToken);
    }
}
