using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Invertory.CreateInvertoryItem;
using ECommerceOrderManagement.Application.Features.Invertory.CreateInventoryItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Invertory;

[Route("api/products/{productId:guid}/inventory")]
public sealed class ProductInventoryController(ISender sender) : BaseApiController
{
    [HttpPost]
    [ProducesResponseType<CreateInventoryItemResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateInventoryItem([FromRoute] Guid productId, [FromBody] CreateInventoryItemRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateInventoryItemCommand(
            productId,
            request.InitialQuantity,
            request.ReorderLevel);

        var result = await sender.Send(command, cancellationToken);

        return HandleCreatedResult(result, response => response, response => $"/api/products/{response.ProductId}/inventory");
    }
}