using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Coupons.GetCoupons;

public sealed class GetCouponsQueryValidator : AbstractValidator<GetCouponsQuery>
{

    private static readonly string[] AllowedSortFields =
        [
        "code",
        "discountvalue",
        "minimumorderamount",
        "startsatutc",
        "endsatutc",
        "usagelimit",
        "createdatutc"
        ];

    private static readonly string[] AllowedSortDirections =
    [
        "asc",
        "desc"
    ];


    public GetCouponsQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThan(0)
            .WithErrorCode(CouponValidationErrors.PageNumberInvalidCode)
            .WithMessage(CouponValidationErrors.PageNumberInvalidMessage);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithErrorCode(CouponValidationErrors.PageSizeInvalidCode)
            .WithMessage(CouponValidationErrors.PageSizeInvalidMessage);

        RuleFor(query => query.SearchTerm)
            .MaximumLength(50)
            .When(query => !string.IsNullOrWhiteSpace(query.SearchTerm))
            .WithErrorCode(CouponValidationErrors.SearchTermMaxLengthCode)
            .WithMessage(CouponValidationErrors.SearchTermMaxLengthMessage);

        RuleFor(query => query.DiscountType)
            .IsInEnum()
            .When(query => query.DiscountType.HasValue)
            .WithErrorCode(CouponValidationErrors.DiscountTypeInvalidCode)
            .WithMessage(CouponValidationErrors.DiscountTypeInvalidMessage);

        RuleFor(query => query.SortBy)
            .Must(sortBy =>
                string.IsNullOrWhiteSpace(sortBy)
                ||
                AllowedSortFields.Contains(sortBy.Trim().ToLowerInvariant()))
            .WithErrorCode(CouponValidationErrors.SortFieldInvalidCode)
            .WithMessage(CouponValidationErrors.SortFieldInvalidMessage);

        RuleFor(query => query.SortDirection)
            .Must(sortDirection =>
                string.IsNullOrWhiteSpace(sortDirection)
                ||
                AllowedSortDirections.Contains(sortDirection.Trim().ToLowerInvariant()))
            .WithErrorCode(CouponValidationErrors.SortDirectionInvalidCode)
            .WithMessage(CouponValidationErrors.SortDirectionInvalidMessage);
    }
}