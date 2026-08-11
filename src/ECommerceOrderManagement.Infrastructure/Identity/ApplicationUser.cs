using Microsoft.AspNetCore.Identity;

namespace ECommerceOrderManagement.Infrastructure.Identity;

internal sealed class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private ApplicationUser()
    {
    }

    private ApplicationUser(Guid id, string firstName, string lastName, string email, DateTime createdAtUtc)
    {
        Id = id;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLowerInvariant();
        UserName = Email;
        IsActive = true;
        CreatedAtUtc = createdAtUtc;
    }

    internal static ApplicationUser Create(string firstName, string lastName, string email, DateTime createdAtUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return new ApplicationUser(
            Guid.NewGuid(),
            firstName,
            lastName,
            email,
            createdAtUtc);
    }

    internal void Activate()
    {
        IsActive = true;
    }

    internal void Deactivate()
    {
        IsActive = false;
    }
}