using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Catalog.Products.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ProductValidationErrors.NameRequiredCode)
            .WithMessage(ProductValidationErrors.NameRequiredMessage)
            .MaximumLength(200)
            .WithErrorCode(ProductValidationErrors.NameMaxLengthCode)
            .WithMessage(ProductValidationErrors.NameMaxLengthMessage);

        RuleFor(command => command.Slug)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ProductValidationErrors.SlugRequiredCode)
            .WithMessage(ProductValidationErrors.SlugRequiredMessage)
            .MaximumLength(220)
            .WithErrorCode(ProductValidationErrors.SlugMaxLengthCode)
            .WithMessage(ProductValidationErrors.SlugMaxLengthMessage);

        RuleFor(command => command.Sku)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ProductValidationErrors.SkuRequiredCode)
            .WithMessage(ProductValidationErrors.SkuRequiredMessage)
            .MaximumLength(100)
            .WithErrorCode(ProductValidationErrors.SkuMaxLengthCode)
            .WithMessage(ProductValidationErrors.SkuMaxLengthMessage);

        RuleFor(command => command.PriceAmount)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(ProductValidationErrors.PriceInvalidCode)
            .WithMessage(ProductValidationErrors.PriceInvalidMessage);

        RuleFor(command => command.Currency)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ProductValidationErrors.CurrencyRequiredCode)
            .WithMessage(ProductValidationErrors.CurrencyRequiredMessage)
            .Length(3)
            .WithErrorCode(ProductValidationErrors.CurrencyLengthCode)
            .WithMessage(ProductValidationErrors.CurrencyLengthMessage);

        RuleFor(command => command.CategoryId)
            .NotEmpty()
            .WithErrorCode(ProductValidationErrors.CategoryRequiredCode)
            .WithMessage(ProductValidationErrors.CategoryRequiredMessage);

        RuleFor(command => command.BrandId)
            .Must(brandId => brandId is null || brandId != Guid.Empty)
            .WithErrorCode(ProductValidationErrors.BrandInvalidCode)
            .WithMessage(ProductValidationErrors.BrandInvalidMessage);

        RuleFor(command => command.Description)
            .MaximumLength(4000)
            .WithErrorCode(ProductValidationErrors.DescriptionMaxLengthCode)
            .WithMessage(ProductValidationErrors.DescriptionMaxLengthMessage);

        RuleFor(command => command.MainImageUrl)
            .MaximumLength(2048)
            .WithErrorCode(ProductValidationErrors.MainImageUrlMaxLengthCode)
            .WithMessage(ProductValidationErrors.MainImageUrlMaxLengthMessage);
    }
}