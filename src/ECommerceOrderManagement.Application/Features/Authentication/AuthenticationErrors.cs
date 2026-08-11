using ECommerceOrderManagement.Application.Common.Results;

namespace ECommerceOrderManagement.Application.Features.Authentication;

public static class AuthenticationErrors
{
    public static Error EmailAlreadyExists(string email)
    {
        return Error.Conflict("Authentication.EmailAlreadyExists", $"A user with email '{email}' already exists.");
    }

    public static Error InvalidCredentials()
    {
        return Error.Unauthorized("Authentication.InvalidCredentials", "Invalid email or password.");
    }

    public static Error InvalidRefreshToken()
    {
        return Error.Unauthorized("Authentication.InvalidRefreshToken", "Refresh token is invalid or expired.");
    }
}