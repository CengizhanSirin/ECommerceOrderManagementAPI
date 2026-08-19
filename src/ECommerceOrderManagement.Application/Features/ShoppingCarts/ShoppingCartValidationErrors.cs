namespace ECommerceOrderManagement.Application.Features.ShoppingCarts;

public static class ShoppingCartValidationErrors
{
    public const string ProductIdRequiredCode = "ShoppingCart.ProductId.Required";

    public const string ProductIdRequiredMessage = "Product ID is required.";

    public const string QuantityGreaterThanZeroCode = "ShoppingCart.Quantity.GreaterThanZero";

    public const string QuantityGreaterThanZeroMessage = "Quantity must be greater than zero.";
}