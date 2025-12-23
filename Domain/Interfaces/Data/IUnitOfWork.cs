namespace Domain.Interfaces.Data
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task BeginTransactionAsync(CancellationToken cancellationToken);

        Task CommitAsync(CancellationToken cancellationToken);

        Task RollbackAsync(CancellationToken cancellationToken);

        Task FlushAsync(CancellationToken cancellationToken);

        bool IsTransactionBegun();
    }
}