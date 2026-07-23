using ECommerceOrderManagement.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace ECommerceOrderManagement.Application.Common.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
   where TRequest : notnull
   where TResponse : IResult<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var validationContext = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(validationContext, cancellationToken)));

        var validationErrors = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .Select(failure => new ValidationError(failure.PropertyName, failure.ErrorCode, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (validationErrors.Length == 0)
        {
            return await next();
        }

        var error = Error.Validation(validationErrors);

        return TResponse.Failure(error);
    }
}
