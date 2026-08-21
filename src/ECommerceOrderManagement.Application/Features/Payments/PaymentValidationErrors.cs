namespace ECommerceOrderManagement.Application.Features.Payments;

public static class PaymentValidationErrors
{
    public const string OrderIdRequiredCode = "Payment.OrderId.Required";

    public const string OrderIdRequiredMessage = "Order ID is required.";

    public const string CardHolderNameRequiredCode = "Payment.CardHolderName.Required";

    public const string CardHolderNameRequiredMessage = "Card holder name is required.";

    public const string CardNumberInvalidCode = "Payment.CardNumber.Invalid";

    public const string CardNumberInvalidMessage = "A valid card number is required.";

    public const string ExpireMonthInvalidCode = "Payment.ExpireMonth.Invalid";

    public const string ExpireMonthInvalidMessage = "Expire month must be between 01 and 12.";

    public const string ExpireYearInvalidCode = "Payment.ExpireYear.Invalid";

    public const string ExpireYearInvalidMessage = "Expire year must contain four digits.";

    public const string CvcInvalidCode = "Payment.Cvc.Invalid";

    public const string CvcInvalidMessage = "CVC must contain 3 or 4 digits.";
}