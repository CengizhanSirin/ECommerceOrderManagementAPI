namespace ECommerceOrderManagement.Application.Features.Authentication.CurrentUser;

public sealed record CurrentUserResponse(Guid UserId,string? Email,IReadOnlyCollection<string> Roles);