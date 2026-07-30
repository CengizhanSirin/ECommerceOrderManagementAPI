using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Invertory.GetInventoryByProductId;

public sealed class GetInventoryByProductIdQueryValidator:AbstractValidator<GetInventoryByProductIdQuery>
{
    public GetInventoryByProductIdQueryValidator()
    {
        RuleFor(query => query.ProductId)
       .NotEmpty()
       .WithErrorCode( InventoryValidationErrors.ProductIdRequiredCode) 
       .WithMessage( InventoryValidationErrors.ProductIdRequiredMessage);
    }
}