using Microsoft.AspNetCore.Identity;

namespace ECommerceOrderManagement.Infrastructure.Identity;

internal sealed class ApplicationRole : IdentityRole<Guid>
{
    private ApplicationRole()
    {
    }

    internal ApplicationRole(Guid id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Id = id;
        Name = name.Trim();
        NormalizedName = name.ToUpperInvariant();
        ConcurrencyStamp = id.ToString();
    }
}