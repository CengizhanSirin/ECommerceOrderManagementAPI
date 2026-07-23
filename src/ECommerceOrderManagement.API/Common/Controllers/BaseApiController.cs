using ECommerceOrderManagement.API.Common.Extensions;
using ECommerceOrderManagement.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Common.Controllers;


[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails();
        }

        return Ok();
    }

    protected IActionResult HandleResult<TValue>(Result<TValue> result)
    {
        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails();
        }

        return Ok(result.Value);
    }

    protected IActionResult HandleCreatedResult<TValue, TResponse>(Result<TValue> result, Func<TValue, TResponse> responseFactory, Func<TResponse, string> locationFactory)
    {
        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails();
        }

        var response = responseFactory(result.Value);

        var location = locationFactory(response);

        return Created(location, response);
    }

    protected IActionResult HandleNoContent(Result result)
    {
        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails();
        }

        return NoContent();
    }
}