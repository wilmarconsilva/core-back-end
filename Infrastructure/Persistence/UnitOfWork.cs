using Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistece
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Public methods

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await FlushAsync(cancellationToken);

            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await DisposeTransactionAsync();
            }
        }

        public Task FlushAsync(CancellationToken cancellationToken)
        {
           return _context.SaveChangesAsync(cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }

        public bool IsTransactionBegun()
        {
            return _transaction != null;
        }

        #endregion Public methods

        #region Private methods

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        #endregion Private methods
    }
}