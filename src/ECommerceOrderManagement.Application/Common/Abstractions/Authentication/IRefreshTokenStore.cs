namespace ECommerceOrderManagement.Application.Common.Abstractions.Authentication;

public interface IRefreshTokenStore
{
    Task AddAsync(Guid userId, string tokenHash, DateTime expiresAtUtc, CancellationToken cancellationToken = default);

    Task<StoredRefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);

    Task<bool> RevokeAsync(Guid refreshTokenId, Guid userId, DateTime revokedAtUtc, CancellationToken cancellationToken = default);

    Task<bool> RotateAsync(Guid currentRefreshTokenId, Guid userId, string newTokenHash, DateTime newExpiresAtUtc, DateTime rotatedAtUtc, CancellationToken cancellationToken = default);
}