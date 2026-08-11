namespace ECommerceOrderManagement.Application.Common.Abstractions.Authentication;

public sealed record AccessTokenResult(string Token, DateTime ExpiresAtUtc);