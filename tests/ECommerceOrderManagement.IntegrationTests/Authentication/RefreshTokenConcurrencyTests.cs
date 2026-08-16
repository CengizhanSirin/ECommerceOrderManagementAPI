using ECommerceOrderManagement.Infrastructure.Authentication;
using ECommerceOrderManagement.Infrastructure.Contexts;
using ECommerceOrderManagement.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerceOrderManagement.IntegrationTests.Authentication;

public sealed class RefreshTokenConcurrencyTests
{
    [Fact]
    public async Task RotateAsync_ShouldAllowOnlyOneRotation_WhenSameRefreshTokenIsRotatedConcurrently()
    {
        var connectionString = Environment.GetEnvironmentVariable("ECOMMERCE_INTEGRATION_TEST_CONNECTION_STRING");

        Assert.False(string.IsNullOrWhiteSpace(connectionString), "ECOMMERCE_INTEGRATION_TEST_CONNECTION_STRING is not configured.");

        var baseOptions = new DbContextOptionsBuilder<IdentityDbContext>().UseSqlServer(connectionString).Options;

        var userId = Guid.NewGuid();

        var currentTokenHash = Guid.NewGuid().ToString("N");

        var newTokenHashA = Guid.NewGuid().ToString("N");

        var newTokenHashB = Guid.NewGuid().ToString("N");

        var createdAtUtc = DateTime.UtcNow;
        var expiresAtUtc = createdAtUtc.AddDays(7);
        var rotatedAtUtc = createdAtUtc.AddMinutes(1);

        try
        {
            Guid currentRefreshTokenId;

            await using (var setupContext = new IdentityDbContext(baseOptions))
            {
                var currentRefreshToken = RefreshToken.Create(userId, currentTokenHash, expiresAtUtc, createdAtUtc);

                currentRefreshTokenId = currentRefreshToken.Id;

                setupContext.RefreshTokens.Add(currentRefreshToken);

                await setupContext.SaveChangesAsync();
            }

            var interceptor = new SaveChangesBarrierInterceptor();

            var concurrentOptions = new DbContextOptionsBuilder<IdentityDbContext>()
                    .UseSqlServer(connectionString)
                    .AddInterceptors(interceptor)
                    .Options;

            await using var contextA = new IdentityDbContext(concurrentOptions);

            await using var contextB = new IdentityDbContext(concurrentOptions);

            var storeA = new RefreshTokenStore(contextA, TimeProvider.System);

            var storeB = new RefreshTokenStore(contextB, TimeProvider.System);

            var rotationTaskA = storeA.RotateAsync(
                    currentRefreshTokenId,
                    userId,
                    newTokenHashA,
                    expiresAtUtc,
                    rotatedAtUtc);

            var rotationTaskB = storeB.RotateAsync(
                    currentRefreshTokenId,
                    userId,
                    newTokenHashB,
                    expiresAtUtc,
                    rotatedAtUtc);

            var results = await Task.WhenAll(rotationTaskA, rotationTaskB);

            Assert.Single(results, result => result);

            Assert.Single(results, result => !result);

            await using var verificationContext = new IdentityDbContext(baseOptions);

            var persistedTokens = await verificationContext.RefreshTokens
                    .AsNoTracking()
                    .Where(refreshToken => refreshToken.UserId == userId)
                    .ToListAsync();

            Assert.Equal(2, persistedTokens.Count);

            var persistedCurrentToken = Assert.Single(persistedTokens, refreshToken => refreshToken.Id == currentRefreshTokenId);

            Assert.NotNull(persistedCurrentToken.RevokedAtUtc);

            var activeToken = Assert.Single(persistedTokens, refreshToken => refreshToken.RevokedAtUtc is null);

            var expectedActiveTokenHash = results[0]
                    ? newTokenHashA
                    : newTokenHashB;

            Assert.Equal(expectedActiveTokenHash, activeToken.TokenHash);
        }
        finally
        {
            await using var cleanupContext = new IdentityDbContext(baseOptions);

            await cleanupContext.RefreshTokens
                .Where(refreshToken => refreshToken.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }

    private sealed class SaveChangesBarrierInterceptor : SaveChangesInterceptor
    {
        private readonly TaskCompletionSource<bool> _bothReached = new(TaskCreationOptions.RunContinuationsAsynchronously);

        private int _saveChangesCallCount;

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (Interlocked.Increment(ref _saveChangesCallCount) == 2)
            {
                _bothReached.TrySetResult(true);
            }

            await _bothReached.Task.WaitAsync(cancellationToken);

            return result;
        }
    }
}