using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Catalog.Categories.CreateCategory;
using ECommerceOrderManagement.API.Features.Catalog.Categories.GetCategories;
using ECommerceOrderManagement.API.Features.Catalog.Categories.UpdateCategory;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.CreateCategory;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.DeleteCategory;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategories;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.GetCategoryById;
using ECommerceOrderManagement.Application.Features.Catalog.Categories.UpdateCategory;
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



    [HttpGet("{categoryId:guid}")]
    [ProducesResponseType<GetCategoryByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery(categoryId);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<GetCategoriesItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery(request.SearchTerm, request.IsActive);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }


    [HttpPut("{categoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(
            categoryId,
            request.Name,
            request.Slug,
            request.Description,
            request.ImageUrl,
            request.DisplayOrder);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpDelete("{categoryId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid categoryId, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(categoryId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }
}