using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.ShippingAddressId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.ShippingAddressRequiredCode)
            .WithMessage(OrderValidationErrors.ShippingAddressRequiredMessage);

        RuleFor(command => command.BillingAddressId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.BillingAddressRequiredCode)
            .WithMessage(OrderValidationErrors.BillingAddressRequiredMessage);

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