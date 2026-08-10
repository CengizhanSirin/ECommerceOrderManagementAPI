namespace ECommerceOrderManagement.Application.Features.Authentication.Login;

public sealed record LoginResponse(string AccessToken, DateTime AccessTokenExpiresAtUtc);