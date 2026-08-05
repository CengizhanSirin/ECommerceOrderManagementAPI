namespace ECommerceOrderManagement.Domain.Orders;

public sealed class OrderAddress
{
    public string FullName { get; private set; } = string.Empty;

    public string PhoneNumber { get; private set; } = string.Empty;

    public string Country { get; private set; } = string.Empty;

    public string City { get; private set; } = string.Empty;

    public string District { get; private set; } = string.Empty;

    public string? PostalCode { get; private set; }

    public string AddressLine { get; private set; } = string.Empty;

    private OrderAddress()
    {
    }

    private OrderAddress(string fullName, string phoneNumber, string country, string city, string district, string? postalCode, string addressLine)
    {
        FullName = fullName.Trim();
        PhoneNumber = phoneNumber.Trim();
        Country = country.Trim();
        City = city.Trim();
        District = district.Trim();
        PostalCode = string.IsNullOrWhiteSpace(postalCode)
            ? null
            : postalCode.Trim();
        AddressLine = addressLine.Trim();
    }

    public static OrderAddress Create(string fullName, string phoneNumber, string country, string city, string district, string? postalCode, string addressLine)
    {
        EnsureRequired(fullName, nameof(fullName), "Full name cannot be empty.");

        EnsureRequired(phoneNumber, nameof(phoneNumber), "Phone number cannot be empty.");

        EnsureRequired(country, nameof(country), "Country cannot be empty.");

        EnsureRequired(city, nameof(city), "City cannot be empty.");

        EnsureRequired(district, nameof(district), "District cannot be empty.");

        EnsureRequired(addressLine, nameof(addressLine), "Address line cannot be empty.");

        return new OrderAddress(fullName, phoneNumber, country, city, district, postalCode, addressLine);
    }

    private static void EnsureRequired(string value, string parameterName, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, parameterName);
        }
    }
}