using ECommerceOrderManagement.Application.Common.Abstractions.Identity;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;

namespace ECommerceOrderManagement.Infrastructure.Identity;

internal sealed class IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
    TimeProvider timeProvider, IdentityDbContext identityDbContext) : IIdentityService
{
    public async Task<IdentityAuthenticationResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedEmail = email.Trim().ToLowerInvariant();

        var user = await userManager.FindByEmailAsync(normalizedEmail);

        if (user is null)
        {
            return new IdentityAuthenticationResult(IdentityAuthenticationStatus.InvalidCredentials, null, null, []);
        }

        if (!user.IsActive)
        {
            return new IdentityAuthenticationResult(IdentityAuthenticationStatus.Inactive, null, null, []);
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (signInResult.IsLockedOut)
        {
            return new IdentityAuthenticationResult(IdentityAuthenticationStatus.LockedOut, null, null, []);
        }

        if (!signInResult.Succeeded)
        {
            return new IdentityAuthenticationResult(IdentityAuthenticationStatus.InvalidCredentials, null, null, []);
        }

        var roles = await userManager.GetRolesAsync(user);

        return new IdentityAuthenticationResult(IdentityAuthenticationStatus.Succeeded, user.Id, user.Email, roles.ToArray());
    }

    public async Task<IdentityUserCreationResult> CreateUserAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await using var transaction = await identityDbContext.Database.BeginTransactionAsync(cancellationToken);

        var user = ApplicationUser.Create(firstName, lastName, email, timeProvider.GetUtcNow().UtcDateTime);

        var identityResult = await userManager.CreateAsync(user, password);

        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors.Select(error =>
                    new IdentityServiceError(error.Code, error.Description)).ToArray();

            await transaction.RollbackAsync(CancellationToken.None);


            return new IdentityUserCreationResult(false, null, errors);
        }

        var roleResult = await userManager.AddToRoleAsync(user, ApplicationRoles.Customer);

        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(error =>
                    new IdentityServiceError(error.Code, error.Description)).ToArray();


            await transaction.RollbackAsync(CancellationToken.None);


            return new IdentityUserCreationResult(false, null, errors);
        }

        await transaction.CommitAsync(cancellationToken);

        return new IdentityUserCreationResult(true, user.Id, []);
    }
}