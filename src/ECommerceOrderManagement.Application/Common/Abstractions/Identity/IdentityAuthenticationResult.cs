namespace ECommerceOrderManagement.Application.Common.Abstractions.Identity;

public sealed record IdentityAuthenticationResult(IdentityAuthenticationStatus Status, Guid? UserId, string? Email, IReadOnlyCollection<string> Roles);