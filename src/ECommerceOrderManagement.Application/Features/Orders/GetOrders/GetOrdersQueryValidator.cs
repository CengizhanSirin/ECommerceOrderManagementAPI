using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Orders.GetOrders;

public sealed class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    private static readonly HashSet<string> AllowedSortFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "orderNumber",
        "status",
        "itemCount",
        "totalAmount",
        "createdAtUtc"
    };


    private static readonly HashSet<string> AllowedSortDirections = new(StringComparer.OrdinalIgnoreCase)
    {
        "asc",
        "desc"
    };

    public GetOrdersQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThan(0)
            .WithErrorCode(OrderValidationErrors.PageNumberMustBePositiveCode)
            .WithMessage(OrderValidationErrors.PageNumberMustBePositiveMessage);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode(OrderValidationErrors.PageSizeOutOfRangeCode)
            .WithMessage(OrderValidationErrors.PageSizeOutOfRangeMessage);

        RuleFor(query => query.SearchTerm)
            .MaximumLength(50)
            .WithErrorCode(OrderValidationErrors.SearchTermTooLongCode)
            .WithMessage(OrderValidationErrors.SearchTermTooLongMessage)
            .When(query => !string.IsNullOrWhiteSpace(query.SearchTerm));

        RuleFor(query => query.CustomerId)
            .Must(customerId => !customerId.HasValue || customerId.Value != Guid.Empty)
            .WithErrorCode(OrderValidationErrors.CustomerIdInvalidCode)
            .WithMessage(OrderValidationErrors.CustomerIdInvalidMessage);

        RuleFor(query => query.Status)
            .Must(status => !status.HasValue || Enum.IsDefined(status.Value))
            .WithErrorCode(OrderValidationErrors.StatusInvalidCode)
            .WithMessage(OrderValidationErrors.StatusInvalidMessage);

        RuleFor(query => query.SortBy)
            .Must(BeValidSortField)
            .WithErrorCode(OrderValidationErrors.SortByInvalidCode)
            .WithMessage(OrderValidationErrors.SortByInvalidMessage);

        RuleFor(query => query.SortDirection)
            .Must(BeValidSortDirection)
            .WithErrorCode(OrderValidationErrors.SortDirectionInvalidCode)
            .WithMessage(OrderValidationErrors.SortDirectionInvalidMessage);
    }

    private static bool BeValidSortField(string? sortBy)
    {
        return string.IsNullOrWhiteSpace(sortBy) || AllowedSortFields.Contains(sortBy);
    }

    private static bool BeValidSortDirection(string? sortDirection)
    {
        return string.IsNullOrWhiteSpace(sortDirection) || AllowedSortDirections.Contains(sortDirection);
    }
}