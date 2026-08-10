using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.ShippingAddress)
            .NotNull()
            .WithErrorCode(OrderValidationErrors.ShippingAddressRequiredCode)
            .WithMessage(OrderValidationErrors.ShippingAddressRequiredMessage)
            .SetValidator(new CreateOrderAddressValidator());

        RuleFor(command => command.BillingAddress)
            .NotNull()
            .WithErrorCode(OrderValidationErrors.BillingAddressRequiredCode)
            .WithMessage(OrderValidationErrors.BillingAddressRequiredMessage)
            .SetValidator(new CreateOrderAddressValidator());

        RuleFor(command => command.Items)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.ItemsRequiredCode)
            .WithMessage(OrderValidationErrors.ItemsRequiredMessage);

        RuleForEach(command => command.Items)
            .SetValidator(new CreateOrderItemValidator());

        RuleFor(command => command.Items)
            .Must(HaveUniqueProductIds)
            .WithErrorCode(OrderValidationErrors.DuplicateProductsCode)
            .WithMessage(OrderValidationErrors.DuplicateProductsMessage);
    }

    private static bool HaveUniqueProductIds(IReadOnlyCollection<CreateOrderItem>? items)
    {
        if (items is null || items.Count == 0)
        {
            return true;
        }

        return items.Select(item => item.ProductId).Distinct().Count() == items.Count;
    }
}