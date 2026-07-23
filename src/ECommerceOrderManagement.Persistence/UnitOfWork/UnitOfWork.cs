using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerceOrderManagement.Persistence.UnitOfWork;

internal sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly ApplicationDbContext _dbContext;

    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException("An active database transaction already exists.");
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("There is no active database transaction to commit.");
        }

        await _currentTransaction.CommitAsync(cancellationToken);

        await DisposeCurrentTransactionAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeCurrentTransactionAsync();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("There is no active database transaction to rollback.");
        }

        await _currentTransaction.RollbackAsync(cancellationToken);

        await DisposeCurrentTransactionAsync();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async ValueTask DisposeCurrentTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        await _currentTransaction.DisposeAsync();

        _currentTransaction = null;
    }
}