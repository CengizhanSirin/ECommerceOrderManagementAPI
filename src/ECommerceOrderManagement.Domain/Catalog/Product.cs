using ECommerceOrderManagement.Domain.Common;
using ECommerceOrderManagement.Domain.ValueObjects;

namespace ECommerceOrderManagement.Domain.Catalog
{
    public sealed class Product : SoftDeletableAggregateRoot
    {
        public string Name { get; private set; } = string.Empty;

        public string Slug { get; private set; } = string.Empty;

        public string Sku { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public string? MainImageUrl { get; private set; }

        public Money Price { get; private set; } = null!;

        public Guid CategoryId { get; private set; }

        public Guid? BrandId { get; private set; }

        public bool IsActive { get; private set; }

        private Product()
        {
        }

        private Product(string name, string slug, string sku, string? description, string? mainImageUrl, Money price, Guid categoryId, Guid? brandId)
        {
            Name = name;
            Slug = slug;
            Sku = sku;
            Description = description;
            MainImageUrl = mainImageUrl;
            Price = price;
            CategoryId = categoryId;
            BrandId = brandId;
            IsActive = true;
        }

        public static Product Create(string name, string slug, string sku, Money price, Guid categoryId, Guid? brandId = null, string? description = null, string? mainImageUrl = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(slug);
            ArgumentException.ThrowIfNullOrWhiteSpace(sku);
            ArgumentNullException.ThrowIfNull(price);

            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("Category id cannot be empty.", nameof(categoryId));
            }

            if (brandId == Guid.Empty)
            {
                throw new ArgumentException("Brand id cannot be empty.", nameof(brandId));
            }

            return new Product(
                name.Trim(),
                slug.Trim().ToLowerInvariant(),
                sku.Trim().ToUpperInvariant(),
                NormalizeOptionalText(description),
                NormalizeOptionalText(mainImageUrl),
                price,
                categoryId,
                brandId);
        }

        public void Update(string name, string slug, string sku, Money price, Guid categoryId, Guid? brandId, string? description, string? mainImageUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(slug);
            ArgumentException.ThrowIfNullOrWhiteSpace(sku);
            ArgumentNullException.ThrowIfNull(price);

            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));
            }

            if (brandId.HasValue && brandId.Value == Guid.Empty)
            {
                throw new ArgumentException("Brand ID cannot be empty.", nameof(brandId));
            }

            Name = name.Trim();

            Slug = slug.Trim().ToLowerInvariant();

            Sku = sku.Trim().ToUpperInvariant();

            Price = price;
            CategoryId = categoryId;
            BrandId = brandId;

            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();

            MainImageUrl = string.IsNullOrWhiteSpace(mainImageUrl)
                ? null
                : mainImageUrl.Trim();
        }

        public void UpdateDetails(string name, string slug, string sku, string? description, string? mainImageUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(slug);
            ArgumentException.ThrowIfNullOrWhiteSpace(sku);

            Name = name.Trim();
            Slug = slug.Trim().ToLowerInvariant();
            Sku = sku.Trim().ToUpperInvariant();
            Description = NormalizeOptionalText(description);
            MainImageUrl = NormalizeOptionalText(mainImageUrl);
        }

        public void ChangePrice(Money price)
        {
            ArgumentNullException.ThrowIfNull(price);

            Price = price;
        }

        public void ChangeCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("Category id cannot be empty.", nameof(categoryId));
            }
            CategoryId = categoryId;
        }

        public void ChangeBrand(Guid brandId)
        {
            if (brandId == Guid.Empty)
            {
                throw new ArgumentException("Brand id cannot be empty.", nameof(brandId));
            }

            BrandId = brandId;
        }

        public void RemoveBrand()
        {
            BrandId = null;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        private static string? NormalizeOptionalText(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }
    }
}
