using ECommerceOrderManagement.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceOrderManagement.Persistence.Configurations.Catalog;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", "catalog");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id)
            .ValueGeneratedNever();

        builder.Property(product => product.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(product => product.Slug)
            .HasMaxLength(220)
            .IsRequired();

        builder.Property(product => product.Sku)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasMaxLength(4000);

        builder.Property(product => product.MainImageUrl)
            .HasMaxLength(2048);

        builder.Property(product => product.CategoryId)
            .IsRequired();

        builder.Property(product => product.BrandId);

        builder.Property(product => product.IsActive)
            .IsRequired();

        builder.Property(product => product.CreatedAtUtc)
            .IsRequired();

        builder.Property(product => product.UpdatedAtUtc);

        builder.Property(product => product.IsDeleted)
            .IsRequired();

        builder.Property(product => product.DeletedAtUtc);

        ConfigurePrice(builder);
        ConfigureRelationships(builder);
        ConfigureIndexes(builder);

        builder.HasQueryFilter(product => !product.IsDeleted);

        builder.Ignore(product => product.DomainEvents);
    }

    private static void ConfigurePrice(EntityTypeBuilder<Product> builder)
    {
        var priceBuilder = builder.ComplexProperty(product => product.Price);
           

        priceBuilder.IsRequired();

        priceBuilder.Property(money => money.Amount)
            .HasColumnName("PriceAmount")
            .HasPrecision(18, 2)
            .IsRequired();

        priceBuilder.Property(money => money.Currency)
            .HasColumnName("PriceCurrency")
            .HasMaxLength(3)
            .IsFixedLength()
            .IsUnicode(false)
            .IsRequired();
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Brand>()
            .WithMany()
            .HasForeignKey(product => product.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureIndexes(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(product => product.Sku)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("UX_Products_Sku_NotDeleted");

        builder.HasIndex(product => product.Slug)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0")
            .HasDatabaseName("UX_Products_Slug_NotDeleted");

        builder.HasIndex(product => new
        {
            product.CategoryId,
            product.IsActive
        }).HasFilter("[IsDeleted] = 0")
           .HasDatabaseName("IX_Products_CategoryId_IsActive_NotDeleted");


        builder.HasIndex(product => new
        {
            product.BrandId,
            product.IsActive
        }).HasFilter("[IsDeleted] = 0 AND [BrandId] IS NOT NULL")
           .HasDatabaseName("IX_Products_BrandId_IsActive_NotDeleted");
    }
}

