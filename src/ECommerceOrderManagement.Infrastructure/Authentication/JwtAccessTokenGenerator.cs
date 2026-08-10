using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceOrderManagement.Infrastructure.Authentication;

internal sealed class JwtAccessTokenGenerator(IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider) : IAccessTokenGenerator
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public AccessTokenResult Generate(Guid userId, string email, IReadOnlyCollection<string> roles)
    {
        var now = timeProvider.GetUtcNow();
        var expiresAtUtc = now.AddMinutes(_jwtOptions.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,userId.ToString()),

            new( JwtRegisteredClaimNames.Email, email),

            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAtUtc.UtcDateTime,
            signingCredentials: signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return new AccessTokenResult(tokenValue, expiresAtUtc.UtcDateTime);
    }
}