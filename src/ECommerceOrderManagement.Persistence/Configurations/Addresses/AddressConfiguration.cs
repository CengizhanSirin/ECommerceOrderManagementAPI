using ECommerceOrderManagement.Domain.Addresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Addresses;

internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(
        EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses", "users");

        builder.HasKey(address => address.Id);

        builder.Property(address => address.UserId)
            .IsRequired();

        builder.Property(address => address.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(address => address.FullName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(address => address.PhoneNumber)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(address => address.Country)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(address => address.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(address => address.District)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(address => address.PostalCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(address => address.AddressLine)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(address => address.IsDefault)
            .IsRequired();

        builder.HasIndex(address => address.UserId, "IX_Addresses_UserId");

        builder.HasIndex(address => address.UserId, "UX_Addresses_UserId_Default_NotDeleted")
            .IsUnique()
            .HasFilter("[IsDefault] = 1 AND [IsDeleted] = 0");

        builder.HasQueryFilter(address => !address.IsDeleted);
    }
}