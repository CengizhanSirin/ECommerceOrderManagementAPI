using FluentValidation;

namespace ECommerceOrderManagement.Application.Features.Payments.ProcessPayment;

public sealed class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEmpty()
            .WithErrorCode(PaymentValidationErrors.OrderIdRequiredCode)
            .WithMessage(PaymentValidationErrors.OrderIdRequiredMessage);

        RuleFor(command => command.Amount)
            .GreaterThan(0)
            .WithErrorCode(PaymentValidationErrors.AmountGreaterThanZeroCode)
            .WithMessage(PaymentValidationErrors.AmountGreaterThanZeroMessage);

        RuleFor(command => command.CardHolderName)
            .NotEmpty()
            .WithErrorCode(PaymentValidationErrors.CardHolderNameRequiredCode)
            .WithMessage(PaymentValidationErrors.CardHolderNameRequiredMessage);

        RuleFor(command => command.CardNumber)
            .NotEmpty()
            .CreditCard()
            .WithErrorCode(PaymentValidationErrors.CardNumberInvalidCode)
            .WithMessage(PaymentValidationErrors.CardNumberInvalidMessage);

        RuleFor(command => command.ExpireMonth)
            .NotEmpty()
            .Matches(@"^(0[1-9]|1[0-2])$")
            .WithErrorCode(PaymentValidationErrors.ExpireMonthInvalidCode)
            .WithMessage(PaymentValidationErrors.ExpireMonthInvalidMessage);

        RuleFor(command => command.ExpireYear)
            .NotEmpty()
            .Matches(@"^\d{4}$")
            .WithErrorCode(PaymentValidationErrors.ExpireYearInvalidCode)
            .WithMessage(PaymentValidationErrors.ExpireYearInvalidMessage);

        RuleFor(command => command.Cvc)
            .NotEmpty()
            .Matches(@"^\d{3,4}$")
            .WithErrorCode(PaymentValidationErrors.CvcInvalidCode)
            .WithMessage(PaymentValidationErrors.CvcInvalidMessage);
    }
}