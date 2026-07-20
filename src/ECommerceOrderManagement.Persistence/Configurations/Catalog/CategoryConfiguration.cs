using ECommerceOrderManagement.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Catalog;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "catalog");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Id)
            .ValueGeneratedNever();

        builder.Property(category => category.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(category => category.Slug)
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(category => category.Description)
            .HasMaxLength(1000);

        builder.Property(category => category.ImageUrl)
            .HasMaxLength(2048);

        builder.Property(category => category.DisplayOrder)
            .IsRequired();

        builder.Property(category => category.IsActive)
            .IsRequired();

        builder.Property(category => category.CreatedAtUtc)
            .IsRequired();

        builder.Property(category => category.UpdatedAtUtc);

        builder.Property(category => category.IsDeleted)
            .IsRequired();

        builder.Property(category => category.DeletedAtUtc);

        builder.HasIndex(category => category.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(category => category.Slug)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasIndex(category => new
        {
            category.IsActive,
            category.DisplayOrder
        }).HasFilter("[IsDeleted] = 0");


        builder.HasQueryFilter(category => !category.IsDeleted);

        builder.Ignore(entity => entity.DomainEvents);
    }
}
