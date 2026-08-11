using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Infrastructure.Configurations;

internal sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {

        builder.ToTable("Roles", "identity");

        builder.HasData(

            new ApplicationRole(Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ApplicationRoles.Admin),

            new ApplicationRole(Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ApplicationRoles.Customer));
    }
}