using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Catalog.Categories.CreateCategory;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Catalog.Categories;

[Route("api/categories")]
public sealed class CategoriesController(ISender sender) : BaseApiController
{
    [HttpPost]
    [ProducesResponseType<CreateCategoryResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(
            request.Name,
            request.Slug,
            request.Description,
            request.ImageUrl,
            request.DisplayOrder);

        var result = await sender.Send(command, cancellationToken);

        return HandleCreatedResult(result, response => response, response => $"/api/categories/{response.Id}");
    }
}