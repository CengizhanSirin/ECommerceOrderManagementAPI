using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Addresses.CreateAddress;

public sealed class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(command => command.Title)
             .NotEmpty()
             .WithErrorCode(AddressValidationErrors.TitleRequiredCode)
             .WithMessage(AddressValidationErrors.TitleRequiredMessage)
             .MaximumLength(50)
             .WithErrorCode(AddressValidationErrors.TitleMaxLengthCode)
             .WithMessage(AddressValidationErrors.TitleMaxLengthMessage);

        RuleFor(command => command.FullName)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.FullNameRequiredCode)
            .WithMessage(AddressValidationErrors.FullNameRequiredMessage)

            .MaximumLength(150)
            .WithErrorCode(AddressValidationErrors.FullNameMaxLengthCode)
            .WithMessage(AddressValidationErrors.FullNameMaxLengthMessage);

        RuleFor(command => command.PhoneNumber)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.PhoneNumberRequiredCode)
            .WithMessage(AddressValidationErrors.PhoneNumberRequiredMessage)

            .MaximumLength(30)
            .WithErrorCode(AddressValidationErrors.PhoneNumberMaxLengthCode)
            .WithMessage(AddressValidationErrors.PhoneNumberMaxLengthMessage);

        RuleFor(command => command.Country)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.CountryRequiredCode)
            .WithMessage(AddressValidationErrors.CountryRequiredMessage)

            .MaximumLength(100)
            .WithErrorCode(AddressValidationErrors.CountryMaxLengthCode)
            .WithMessage(AddressValidationErrors.CountryMaxLengthMessage);

        RuleFor(command => command.City)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.CityRequiredCode)
            .WithMessage(AddressValidationErrors.CityRequiredMessage)

            .MaximumLength(100)
            .WithErrorCode(AddressValidationErrors.CityMaxLengthCode)
            .WithMessage(AddressValidationErrors.CityMaxLengthMessage);

        RuleFor(command => command.District)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.DistrictRequiredCode)
            .WithMessage(AddressValidationErrors.DistrictRequiredMessage)

            .MaximumLength(100)
            .WithErrorCode(AddressValidationErrors.DistrictMaxLengthCode)
            .WithMessage(AddressValidationErrors.DistrictMaxLengthMessage);

        RuleFor(command => command.PostalCode)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.PostalCodeRequiredCode)
            .WithMessage(AddressValidationErrors.PostalCodeRequiredMessage)

            .MaximumLength(20)
            .WithErrorCode(AddressValidationErrors.PostalCodeMaxLengthCode)
            .WithMessage(AddressValidationErrors.PostalCodeMaxLengthMessage);

        RuleFor(command => command.AddressLine)
            .NotEmpty()
            .WithErrorCode(AddressValidationErrors.AddressLineRequiredCode)
            .WithMessage(AddressValidationErrors.AddressLineRequiredMessage)

            .MaximumLength(500)
            .WithErrorCode(AddressValidationErrors.AddressLineMaxLengthCode)
            .WithMessage(AddressValidationErrors.AddressLineMaxLengthMessage);
    }
}