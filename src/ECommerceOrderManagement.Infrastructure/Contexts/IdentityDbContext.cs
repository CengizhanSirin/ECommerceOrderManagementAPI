using ECommerceOrderManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceOrderManagement.Infrastructure.Contexts;

internal sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("UserRoles","identity");

        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("UserClaims","identity");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("UserLogins","identity");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("RoleClaims","identity");

        modelBuilder.Entity<IdentityUserToken<Guid>>()
            .ToTable("UserTokens","identity");
    }
}