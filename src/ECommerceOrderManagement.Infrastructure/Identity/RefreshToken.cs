namespace ECommerceOrderManagement.Infrastructure.Identity;

internal sealed class RefreshToken
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? RevokedAtUtc { get; private set; }

    public byte[] RowVersion { get; private set; } = [];

    private RefreshToken()
    {
    }

    private RefreshToken(Guid id, Guid userId, string tokenHash, DateTime expiresAtUtc, DateTime createdAtUtc)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = createdAtUtc;
    }

    internal static RefreshToken Create(Guid userId, string tokenHash, DateTime expiresAtUtc, DateTime createdAtUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenHash);

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User identifier cannot be empty.", nameof(userId));
        }

        return new RefreshToken(Guid.NewGuid(), userId, tokenHash, expiresAtUtc, createdAtUtc);
    }

    internal void Revoke(DateTime revokedAtUtc)
    {
        RevokedAtUtc = revokedAtUtc;
    }
}