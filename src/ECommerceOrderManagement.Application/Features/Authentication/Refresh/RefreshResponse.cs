namespace ECommerceOrderManagement.Application.Features.Authentication.Refresh;

public sealed record RefreshResponse(string AccessToken, DateTime AccessTokenExpiresAtUtc, string RefreshToken, DateTime RefreshTokenExpiresAtUtc);