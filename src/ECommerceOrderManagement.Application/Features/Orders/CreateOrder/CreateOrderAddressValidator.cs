using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

internal sealed class CreateOrderAddressValidator : AbstractValidator<CreateOrderAddress>
{
    public CreateOrderAddressValidator()
    {
        RuleFor(address => address.FullName)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.FullNameRequiredCode)
            .WithMessage(OrderValidationErrors.FullNameRequiredMessage)
            .MaximumLength(150)
            .WithErrorCode(OrderValidationErrors.FullNameTooLongCode)
            .WithMessage(OrderValidationErrors.FullNameTooLongMessage);

        RuleFor(address => address.PhoneNumber)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.PhoneNumberRequiredCode)
            .WithMessage(OrderValidationErrors.PhoneNumberRequiredMessage)
            .MaximumLength(30)
            .WithErrorCode(OrderValidationErrors.PhoneNumberTooLongCode)
            .WithMessage(OrderValidationErrors.PhoneNumberTooLongMessage);

        RuleFor(address => address.Country)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.CountryRequiredCode)
            .WithMessage(OrderValidationErrors.CountryRequiredMessage)
            .MaximumLength(100)
            .WithErrorCode(OrderValidationErrors.CountryTooLongCode)
            .WithMessage(OrderValidationErrors.CountryTooLongMessage);

        RuleFor(address => address.City)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.CityRequiredCode)
            .WithMessage(OrderValidationErrors.CityRequiredMessage)
            .MaximumLength(100)
            .WithErrorCode(OrderValidationErrors.CityTooLongCode)
            .WithMessage(OrderValidationErrors.CityTooLongMessage);

        RuleFor(address => address.District)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.DistrictRequiredCode)
            .WithMessage(OrderValidationErrors.DistrictRequiredMessage)
            .MaximumLength(100)
            .WithErrorCode(OrderValidationErrors.DistrictTooLongCode)
            .WithMessage(OrderValidationErrors.DistrictTooLongMessage);

        RuleFor(address => address.PostalCode)
            .MaximumLength(20)
            .WithErrorCode(OrderValidationErrors.PostalCodeTooLongCode)
            .WithMessage(OrderValidationErrors.PostalCodeTooLongMessage);

        RuleFor(address => address.AddressLine)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.AddressLineRequiredCode)
            .WithMessage(OrderValidationErrors.AddressLineRequiredMessage)
            .MaximumLength(500)
            .WithErrorCode(OrderValidationErrors.AddressLineTooLongCode)
            .WithMessage(OrderValidationErrors.AddressLineTooLongMessage);
    }
}