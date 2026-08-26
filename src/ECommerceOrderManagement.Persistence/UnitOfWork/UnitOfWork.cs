using ECommerceOrderManagement.Application.Common.Abstractions.Persistence;
using ECommerceOrderManagement.Application.Common.Exceptions;
using ECommerceOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

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

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException("An active database transaction already exists.");
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
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

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);

        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyConflictException("The data was modified by another operation.", exception);
        }
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