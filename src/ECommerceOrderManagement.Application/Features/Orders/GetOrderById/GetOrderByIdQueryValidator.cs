using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrderById;

public sealed class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(query => query.OrderId)
            .NotEmpty()
            .WithErrorCode(OrderValidationErrors.OrderIdRequiredCode)
            .WithMessage(OrderValidationErrors.OrderIdRequiredMessage);
    }
}