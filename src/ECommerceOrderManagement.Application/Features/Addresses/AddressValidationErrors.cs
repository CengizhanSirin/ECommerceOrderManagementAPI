namespace ECommerceOrderManagement.Application.Features.Addresses;

public static class AddressValidationErrors
{
    public const string AddressIdRequiredCode = "Address.Id.Required";
    public const string AddressIdRequiredMessage = "Address ID is required.";

    public const string TitleRequiredCode = "Address.Title.Required";
    public const string TitleRequiredMessage = "Address title is required.";

    public const string TitleMaxLengthCode = "Address.Title.MaxLength";
    public const string TitleMaxLengthMessage = "Address title must not exceed 50 characters.";

    public const string FullNameRequiredCode = "Address.FullName.Required";
    public const string FullNameRequiredMessage = "Full name is required.";

    public const string FullNameMaxLengthCode = "Address.FullName.MaxLength";
    public const string FullNameMaxLengthMessage = "Full name must not exceed 150 characters.";

    public const string PhoneNumberRequiredCode = "Address.PhoneNumber.Required";
    public const string PhoneNumberRequiredMessage = "Phone number is required.";

    public const string PhoneNumberMaxLengthCode = "Address.PhoneNumber.MaxLength";
    public const string PhoneNumberMaxLengthMessage = "Phone number must not exceed 30 characters.";

    public const string CountryRequiredCode = "Address.Country.Required";
    public const string CountryRequiredMessage = "Country is required.";

    public const string CountryMaxLengthCode = "Address.Country.MaxLength";
    public const string CountryMaxLengthMessage = "Country must not exceed 100 characters.";

    public const string CityRequiredCode = "Address.City.Required";
    public const string CityRequiredMessage = "City is required.";

    public const string CityMaxLengthCode = "Address.City.MaxLength";
    public const string CityMaxLengthMessage = "City must not exceed 100 characters.";

    public const string DistrictRequiredCode = "Address.District.Required";
    public const string DistrictRequiredMessage = "District is required.";

    public const string DistrictMaxLengthCode = "Address.District.MaxLength";
    public const string DistrictMaxLengthMessage = "District must not exceed 100 characters.";

    public const string PostalCodeRequiredCode = "Address.PostalCode.Required";
    public const string PostalCodeRequiredMessage = "Postal code is required.";

    public const string PostalCodeMaxLengthCode = "Address.PostalCode.MaxLength";
    public const string PostalCodeMaxLengthMessage = "Postal code must not exceed 20 characters.";

    public const string AddressLineRequiredCode = "Address.AddressLine.Required";
    public const string AddressLineRequiredMessage = "Address line is required.";

    public const string AddressLineMaxLengthCode = "Address.AddressLine.MaxLength";
    public const string AddressLineMaxLengthMessage = "Address line must not exceed 500 characters.";
}