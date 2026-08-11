namespace ECommerceOrderManagement.Application.Common.Abstractions.Identity;

public sealed record IdentityUserInfo(Guid UserId, string Email, bool IsActive, IReadOnlyCollection<string> Roles);