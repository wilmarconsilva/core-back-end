namespace Application.Shared.Services.Interfaces
{
    public interface IReportService<TReport, TFilter>
    {
        Task<TReport> Get(TFilter filter, CancellationToken cancellationToken);
    }
}