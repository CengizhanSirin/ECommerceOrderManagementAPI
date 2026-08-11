namespace ECommerceOrderManagement.Application.Common.Abstractions.Authentication;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    Guid UserId { get; }

    string? Email { get; }

    IReadOnlyCollection<string> Roles { get; }
}