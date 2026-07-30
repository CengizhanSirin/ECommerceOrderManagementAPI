using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Invertory.CreateInvertoryItem;
using ECommerceOrderManagement.API.Features.Invertory.IncreaseStock;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;
using ECommerceOrderManagement.Application.Features.Invertory.CreateInventoryItem;
using ECommerceOrderManagement.Application.Features.Invertory.GetInventoryByProductId;
using ECommerceOrderManagement.Application.Features.Invertory.IncreaseStock;
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

  

    [HttpGet]
    [ProducesResponseType<GetInventoryByProductIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInventoryByProductId([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        var query = new GetInventoryByProductIdQuery(productId);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpPost("increase")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> IncreaseStock([FromRoute] Guid productId, [FromBody] IncreaseStockRequest request, CancellationToken cancellationToken)
    {
        var command = new IncreaseStockCommand(productId, request.Quantity);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }
}