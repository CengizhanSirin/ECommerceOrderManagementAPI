using ECommerceOrderManagement.Domain.Common;

namespace ECommerceOrderManagement.Domain.Addresses;

public sealed class Address : SoftDeletableAggregateRoot
{
    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string FullName { get; private set; } = string.Empty;

    public string PhoneNumber { get; private set; } = string.Empty;

    public string Country { get; private set; } = string.Empty;

    public string City { get; private set; } = string.Empty;

    public string District { get; private set; } = string.Empty;

    public string PostalCode { get; private set; } = string.Empty;

    public string AddressLine { get; private set; } = string.Empty;

    private Address()
    {
    }

    private Address(Guid id, Guid userId, string title, string fullName, string phoneNumber, string country, string city, string district, string postalCode, string addressLine)
        : base(id)
    {
        UserId = userId;
        Title = title;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Country = country;
        City = city;
        District = district;
        PostalCode = postalCode;
        AddressLine = addressLine;
    }


    public static Address Create(Guid userId, string title, string fullName, string phoneNumber, string country, string city, string district, string postalCode, string addressLine)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User identifier cannot be empty.", nameof(userId));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        ArgumentException.ThrowIfNullOrWhiteSpace(district);
        ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        return new Address(Guid.NewGuid(), userId, title.Trim(), fullName.Trim(), phoneNumber.Trim(), country.Trim(), city.Trim(), district.Trim(), postalCode.Trim(),
            addressLine.Trim());
    }

    public void Update(string title, string fullName, string phoneNumber, string country, string city, string district, string postalCode, string addressLine)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);
        ArgumentException.ThrowIfNullOrWhiteSpace(city);
        ArgumentException.ThrowIfNullOrWhiteSpace(district);
        ArgumentException.ThrowIfNullOrWhiteSpace(postalCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        Title = title.Trim();
        FullName = fullName.Trim();
        PhoneNumber = phoneNumber.Trim();
        Country = country.Trim();
        City = city.Trim();
        District = district.Trim();
        PostalCode = postalCode.Trim();
        AddressLine = addressLine.Trim();
    }
}