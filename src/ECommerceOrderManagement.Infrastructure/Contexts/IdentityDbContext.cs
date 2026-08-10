using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Infrastructure.Contexts;

internal sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(
            userBuilder =>
            {
                userBuilder.ToTable(
                    "Users",
                    "identity");

                userBuilder.Property(user => user.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                userBuilder.Property(user => user.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                userBuilder.Property(user => user.IsActive)
                    .IsRequired();

                userBuilder.Property(user => user.CreatedAtUtc)
                    .IsRequired();
            });

        builder.Entity<ApplicationRole>()
            .ToTable(
                "Roles",
                "identity");

        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole(Guid.Parse("11111111-1111-1111-1111-111111111111"), ApplicationRoles.Admin),
            new ApplicationRole(Guid.Parse("22222222-2222-2222-2222-222222222222"), ApplicationRoles.Customer));

        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable(
                "UserRoles",
                "identity");

        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable(
                "UserClaims",
                "identity");

        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable(
                "UserLogins",
                "identity");

        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable(
                "RoleClaims",
                "identity");

        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable(
                "UserTokens",
                "identity");
    }
}