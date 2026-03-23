namespace Application.Interfaces.Services
{
    public interface IReportService<TReport, TFilter>
    {
        Task<TReport> Get(TFilter filter, CancellationToken cancellationToken);
    }
}