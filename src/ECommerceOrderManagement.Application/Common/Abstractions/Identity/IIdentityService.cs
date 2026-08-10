namespace ECommerceOrderManagement.Application.Common.Abstractions.Identity;

public interface IIdentityService
{
    Task<IdentityUserCreationResult> CreateUserAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken = default);

    Task<IdentityAuthenticationResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
}