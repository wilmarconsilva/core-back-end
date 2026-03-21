namespace Application.Interfaces.Queries
{
    public interface IReportQuery<TReport, TFilter>
    {
        Task<List<TReport>> Get(TFilter filter, CancellationToken cancellationToken);
    }
}
