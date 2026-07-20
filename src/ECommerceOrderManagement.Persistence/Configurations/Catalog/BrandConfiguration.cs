using ECommerceOrderManagement.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Catalog;

public sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands", "catalog");

        builder.HasKey(brand => brand.Id);

        builder.Property(brand => brand.Id)
            .ValueGeneratedNever();

        builder.Property(brand => brand.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(brand => brand.Slug)
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(brand => brand.Description)
            .HasMaxLength(1000);

        builder.Property(brand => brand.LogoUrl)
            .HasMaxLength(2048);

        builder.Property(brand => brand.IsActive)
            .IsRequired();

        builder.Property(brand => brand.CreatedAtUtc)
            .IsRequired();

        builder.Property(brand => brand.UpdatedAtUtc);

        builder.Property(brand => brand.IsDeleted)
            .IsRequired();

        builder.Property(brand => brand.DeletedAtUtc);


        builder.HasIndex(brand => brand.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(brand => brand.Slug)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");


        builder.HasQueryFilter(brand => !brand.IsDeleted);

        builder.Ignore(entity => entity.DomainEvents);
    }
}
