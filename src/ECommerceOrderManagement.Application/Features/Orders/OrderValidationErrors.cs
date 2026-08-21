namespace ECommerceOrderManagement.Application.Features.Orders;

public static class OrderValidationErrors
{
    public const string ShippingAddressRequiredCode = "Orders.ShippingAddressRequired";
    public const string ShippingAddressRequiredMessage = "Shipping address is required.";

    public const string BillingAddressRequiredCode = "Orders.BillingAddressRequired";
    public const string BillingAddressRequiredMessage = "Billing address is required.";

    public const string ItemsRequiredCode = "Orders.ItemsRequired";
    public const string ItemsRequiredMessage = "At least one order item is required.";

    public const string DuplicateProductsCode = "Orders.DuplicateProducts";
    public const string DuplicateProductsMessage = "The same product cannot be added more than once.";

    public const string ProductIdRequiredCode = "Orders.ProductIdRequired";
    public const string ProductIdRequiredMessage = "Product id is required.";

    public const string QuantityMustBePositiveCode = "Orders.QuantityMustBePositive";
    public const string QuantityMustBePositiveMessage = "Quantity must be greater than zero.";

    public const string FullNameRequiredCode = "Orders.FullNameRequired";
    public const string FullNameRequiredMessage = "Full name is required.";

    public const string FullNameTooLongCode = "Orders.FullNameTooLong";
    public const string FullNameTooLongMessage = "Full name cannot exceed 150 characters.";

    public const string PhoneNumberRequiredCode = "Orders.PhoneNumberRequired";
    public const string PhoneNumberRequiredMessage = "Phone number is required.";

    public const string PhoneNumberTooLongCode = "Orders.PhoneNumberTooLong";
    public const string PhoneNumberTooLongMessage = "Phone number cannot exceed 30 characters.";

    public const string CountryRequiredCode = "Orders.CountryRequired";
    public const string CountryRequiredMessage = "Country is required.";

    public const string CountryTooLongCode = "Orders.CountryTooLong";
    public const string CountryTooLongMessage = "Country cannot exceed 100 characters.";

    public const string CityRequiredCode = "Orders.CityRequired";
    public const string CityRequiredMessage = "City is required.";

    public const string CityTooLongCode = "Orders.CityTooLong";
    public const string CityTooLongMessage = "City cannot exceed 100 characters.";

    public const string DistrictRequiredCode = "Orders.DistrictRequired";
    public const string DistrictRequiredMessage = "District is required.";

    public const string DistrictTooLongCode = "Orders.DistrictTooLong";
    public const string DistrictTooLongMessage = "District cannot exceed 100 characters.";

    public const string PostalCodeTooLongCode = "Orders.PostalCodeTooLong";
    public const string PostalCodeTooLongMessage = "Postal code cannot exceed 20 characters.";

    public const string AddressLineRequiredCode = "Orders.AddressLineRequired";
    public const string AddressLineRequiredMessage = "Address line is required.";

    public const string AddressLineTooLongCode = "Orders.AddressLineTooLong";
    public const string AddressLineTooLongMessage = "Address line cannot exceed 500 characters.";

    public const string CancellationReasonRequiredCode = "Orders.CancellationReasonRequired";
    public const string CancellationReasonRequiredMessage = "Cancellation reason is required.";

    public const string CancellationReasonTooLongCode = "Orders.CancellationReasonTooLong";
    public const string CancellationReasonTooLongMessage = "Cancellation reason cannot exceed 500 characters.";

    public const string OrderIdRequiredCode = "Orders.OrderIdRequired";
    public const string OrderIdRequiredMessage = "Order id is required.";

    public const string PageNumberMustBePositiveCode = "Orders.PageNumberMustBePositive";
    public const string PageNumberMustBePositiveMessage = "Page number must be greater than zero.";

    public const string PageSizeOutOfRangeCode = "Orders.PageSizeOutOfRange";
    public const string PageSizeOutOfRangeMessage = "Page size must be between 1 and 100.";

    public const string SearchTermTooLongCode = "Orders.SearchTermTooLong";
    public const string SearchTermTooLongMessage = "Search term cannot exceed 50 characters.";

    public const string StatusInvalidCode = "Orders.StatusInvalid";
    public const string StatusInvalidMessage = "Order status is invalid.";

    public const string SortByInvalidCode = "Orders.SortByInvalid";
    public const string SortByInvalidMessage = "Sort by value is invalid.";

    public const string SortDirectionInvalidCode = "Orders.SortDirectionInvalid";
    public const string SortDirectionInvalidMessage = "Sort direction must be either 'asc' or 'desc'.";
}