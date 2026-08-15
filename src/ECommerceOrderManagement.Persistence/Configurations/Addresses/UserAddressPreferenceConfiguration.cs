using ECommerceOrderManagement.Domain.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Addresses;

internal sealed class UserAddressPreferenceConfiguration : IEntityTypeConfiguration<UserAddressPreference>
{
    public void Configure(EntityTypeBuilder<UserAddressPreference> builder)
    {
        builder.ToTable("UserAddressPreferences", "users");

        builder.HasKey(preference => preference.Id);

        builder.Property(preference => preference.Id)
            .ValueGeneratedNever();

        builder.Property(preference => preference.UserId)
            .IsRequired();

        builder.Property(preference => preference.DefaultShippingAddressId);

        builder.Property(preference => preference.DefaultBillingAddressId);

        builder.Property(preference => preference.CreatedAtUtc)
            .IsRequired();

        builder.Property(preference => preference.UpdatedAtUtc);

        builder.HasIndex(preference => preference.UserId)
            .IsUnique();

        builder.Ignore(preference => preference.DomainEvents);
    }
}