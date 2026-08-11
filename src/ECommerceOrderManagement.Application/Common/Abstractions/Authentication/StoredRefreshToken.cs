namespace ECommerceOrderManagement.Application.Common.Abstractions.Authentication;

public sealed record StoredRefreshToken(Guid Id, Guid UserId, DateTime ExpiresAtUtc, DateTime? RevokedAtUtc);