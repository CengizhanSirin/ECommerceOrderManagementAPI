namespace ECommerceOrderManagement.Application.Common.Abstractions.Identity;

public enum IdentityAuthenticationStatus
{
    Succeeded = 1,
    InvalidCredentials = 2,
    LockedOut = 3,
    Inactive = 4
}