namespace ECommerceOrderManagement.Application.Common.Abstractions.Identity;

public sealed record IdentityUserCreationResult(bool Succeeded, Guid? UserId, IReadOnlyCollection<IdentityServiceError> Errors);