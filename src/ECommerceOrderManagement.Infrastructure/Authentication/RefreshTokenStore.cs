using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using ECommerceOrderManagement.Infrastructure.Contexts;
using ECommerceOrderManagement.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Infrastructure.Authentication;

internal sealed class RefreshTokenStore(IdentityDbContext identityDbContext, TimeProvider timeProvider) : IRefreshTokenStore
{
    public async Task AddAsync(Guid userId, string tokenHash, DateTime expiresAtUtc, CancellationToken cancellationToken = default)
    {
        var refreshToken = RefreshToken.Create(
            userId,
            tokenHash,
            expiresAtUtc,
            timeProvider.GetUtcNow().UtcDateTime);

        identityDbContext.RefreshTokens.Add(refreshToken);

        await identityDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<StoredRefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
    {
        return await identityDbContext.RefreshTokens
        .AsNoTracking()
        .Where(refreshToken => refreshToken.TokenHash == tokenHash)
        .Select(refreshToken => new StoredRefreshToken(
                refreshToken.Id,
                refreshToken.UserId,
                refreshToken.ExpiresAtUtc,
                refreshToken.RevokedAtUtc))
        .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> RevokeAsync(Guid refreshTokenId, Guid userId, DateTime revokedAtUtc, CancellationToken cancellationToken = default)
    {
        var refreshToken = await identityDbContext.RefreshTokens.SingleAsync(
            refreshToken =>
            refreshToken.Id == refreshTokenId &&
            refreshToken.UserId == userId &&
            refreshToken.RevokedAtUtc == null, cancellationToken);


        if (refreshToken is null)
        {
            return false;
        }

        refreshToken.Revoke(revokedAtUtc);

        try
        {
            await identityDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (DbUpdateConcurrencyException)
        {

            return false;
        }

    }

    public async Task<bool> RotateAsync(Guid currentRefreshTokenId, Guid userId, string newTokenHash, DateTime newExpiresAtUtc, DateTime rotatedAtUtc, CancellationToken cancellationToken = default)
    {
        var currentRefreshToken = await identityDbContext.RefreshTokens
           .SingleOrDefaultAsync(
               refreshToken => refreshToken.Id == currentRefreshTokenId && refreshToken.RevokedAtUtc == null, cancellationToken);

        if (currentRefreshToken is null)
        {
            return false;
        }

        currentRefreshToken.Revoke(rotatedAtUtc);

        var newRefreshToken = RefreshToken.Create(
            userId,
            newTokenHash,
            newExpiresAtUtc,
            rotatedAtUtc);

        identityDbContext.RefreshTokens.Add(newRefreshToken);

        try
        {
            await identityDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }
}