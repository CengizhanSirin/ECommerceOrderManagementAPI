using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Addresses.UpdateAddress;

public sealed class UpdateAddressCommandValidator:AbstractValidator<UpdateAddressCommand>
{
    private const int MaximumTitleLength = 50;
    private const int MaximumFullNameLength = 150;
    private const int MaximumPhoneNumberLength = 30;
    private const int MaximumCountryLength = 100;
    private const int MaximumCityLength = 100;
    private const int MaximumDistrictLength = 100;
    private const int MaximumPostalCodeLength = 20;
    private const int MaximumAddressLineLength = 500;

    public UpdateAddressCommandValidator()
    {
        RuleFor(command => command.AddressId)
           .NotEmpty()
           .WithErrorCode(AddressValidationErrors.AddressIdRequiredCode)
           .WithMessage(AddressValidationErrors.AddressIdRequiredMessage);

        RuleFor(command => command.Title)
            .Cascade(CascadeMode.Stop)
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithErrorCode(AddressValidationErrors.TitleRequiredCode)
            .WithMessage(AddressValidationErrors.TitleRequiredMessage)

            .MaximumLength(MaximumTitleLength)
            .WithErrorCode(AddressValidationErrors.TitleMaxLengthCode)
            .WithMessage(AddressValidationErrors.TitleMaxLengthMessage);

        RuleFor(command => command.FullName)
            .Cascade(CascadeMode.Stop)
            .Must(fullName => !string.IsNullOrWhiteSpace(fullName))
            .WithErrorCode(AddressValidationErrors.FullNameRequiredCode)
            .WithMessage(AddressValidationErrors.FullNameRequiredMessage)

            .MaximumLength(MaximumFullNameLength)
            .WithErrorCode(AddressValidationErrors.FullNameMaxLengthCode)
            .WithMessage(AddressValidationErrors.FullNameMaxLengthMessage);

        RuleFor(command => command.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .Must(phoneNumber => !string.IsNullOrWhiteSpace(phoneNumber))
            .WithErrorCode(AddressValidationErrors.PhoneNumberRequiredCode)
            .WithMessage(AddressValidationErrors.PhoneNumberRequiredMessage)

            .MaximumLength(MaximumPhoneNumberLength)
            .WithErrorCode(AddressValidationErrors.PhoneNumberMaxLengthCode)
            .WithMessage(AddressValidationErrors.PhoneNumberMaxLengthMessage);

        RuleFor(command => command.Country)
            .Cascade(CascadeMode.Stop)
            .Must(country => !string.IsNullOrWhiteSpace(country))
            .WithErrorCode(AddressValidationErrors.CountryRequiredCode)
            .WithMessage(AddressValidationErrors.CountryRequiredMessage)

            .MaximumLength(MaximumCountryLength)
            .WithErrorCode(AddressValidationErrors.CountryMaxLengthCode)
            .WithMessage(AddressValidationErrors.CountryMaxLengthMessage);

        RuleFor(command => command.City)
            .Cascade(CascadeMode.Stop)
            .Must(city => !string.IsNullOrWhiteSpace(city))
            .WithErrorCode(AddressValidationErrors.CityRequiredCode)
            .WithMessage(AddressValidationErrors.CityRequiredMessage)

            .MaximumLength(MaximumCityLength)
            .WithErrorCode(AddressValidationErrors.CityMaxLengthCode)
            .WithMessage(AddressValidationErrors.CityMaxLengthMessage);

        RuleFor(command => command.District)
            .Cascade(CascadeMode.Stop)
            .Must(district => !string.IsNullOrWhiteSpace(district))
            .WithErrorCode(AddressValidationErrors.DistrictRequiredCode)
            .WithMessage(AddressValidationErrors.DistrictRequiredMessage)

            .MaximumLength(MaximumDistrictLength)
            .WithErrorCode(AddressValidationErrors.DistrictMaxLengthCode)
            .WithMessage(AddressValidationErrors.DistrictMaxLengthMessage);

        RuleFor(command => command.PostalCode)
            .Cascade(CascadeMode.Stop)
            .Must(postalCode => !string.IsNullOrWhiteSpace(postalCode))
            .WithErrorCode(AddressValidationErrors.PostalCodeRequiredCode)
            .WithMessage(AddressValidationErrors.PostalCodeRequiredMessage)

            .MaximumLength(MaximumPostalCodeLength)
            .WithErrorCode(AddressValidationErrors.PostalCodeMaxLengthCode)
            .WithMessage(AddressValidationErrors.PostalCodeMaxLengthMessage);

        RuleFor(command => command.AddressLine)
            .Cascade(CascadeMode.Stop)
            .Must(addressLine => !string.IsNullOrWhiteSpace(addressLine))
            .WithErrorCode(AddressValidationErrors.AddressLineRequiredCode)
            .WithMessage(AddressValidationErrors.AddressLineRequiredMessage)

            .MaximumLength(MaximumAddressLineLength)
            .WithErrorCode(AddressValidationErrors.AddressLineMaxLengthCode)
            .WithMessage(AddressValidationErrors.AddressLineMaxLengthMessage);
    }
}