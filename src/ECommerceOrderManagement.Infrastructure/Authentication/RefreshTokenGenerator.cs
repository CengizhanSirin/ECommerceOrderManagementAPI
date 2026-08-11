using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceOrderManagement.Infrastructure.Authentication;

internal sealed class RefreshTokenGenerator(IOptions<RefreshTokenOptions> refreshTokenOptions, TimeProvider timeProvider) : IRefreshTokenGenerator
{

    private readonly RefreshTokenOptions _refreshTokenOptions = refreshTokenOptions.Value;

    public RefreshTokenResult Generate()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);

        var token = Convert.ToBase64String(tokenBytes);

        var tokenHash = ComputeHash(token);

        var expiresAtUtc = timeProvider.GetUtcNow().AddDays(_refreshTokenOptions.ExpirationDays).UtcDateTime;

        return new RefreshTokenResult(token, tokenHash, expiresAtUtc);
    }

    public string ComputeHash(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }
}