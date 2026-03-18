namespace Application.Interfaces.Queries
{
    public interface IReportQuery<TReport, TFilter>
    {
        Task<IEnumerable<TReport>> Get(TFilter filter, CancellationToken cancellationToken);
    }
}
