using ECommerceOrderManagement.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Common.Extensions;

public static class ErrorExtensions
{
    public static ObjectResult ToProblemDetails(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,

            ErrorType.NotFound => StatusCodes.Status404NotFound,

            ErrorType.Conflict => StatusCodes.Status409Conflict,

            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,

            ErrorType.Forbidden => StatusCodes.Status403Forbidden,

            ErrorType.Failure => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Code,
            Detail = error.Description
        };

        if (error.ValidationErrors.Count > 0)
        {
            problemDetails.Extensions["validationErrors"] = error.ValidationErrors;
        }

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }
}