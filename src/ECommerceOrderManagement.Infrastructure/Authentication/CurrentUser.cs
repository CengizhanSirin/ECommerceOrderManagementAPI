using ECommerceOrderManagement.Application.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerceOrderManagement.Infrastructure.Authentication;

internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{

    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public string? Email => User?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public IReadOnlyCollection<string> Roles => User?
          .FindAll(ClaimTypes.Role)
          .Select(claim => claim.Value)
          .ToArray()
          ?? [];

    public Guid UserId
    {
        get
        {
            var userId = User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                throw new InvalidOperationException("The authenticated user's identifier is missing or invalid.");
            }

            return parsedUserId;
        }
    }
}