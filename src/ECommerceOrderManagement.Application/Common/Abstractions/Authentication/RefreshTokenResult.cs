namespace ECommerceOrderManagement.Application.Common.Abstractions.Authentication;

public sealed record RefreshTokenResult(string Token, string TokenHash, DateTime ExpiresAtUtc);