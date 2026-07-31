using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Inventory.ChangeReorderLevel;
using ECommerceOrderManagement.API.Features.Inventory.CreateInvertoryItem;
using ECommerceOrderManagement.API.Features.Inventory.DecreaseStock;
using ECommerceOrderManagement.API.Features.Inventory.GetStockMovementHistory;
using ECommerceOrderManagement.API.Features.Inventory.IncreaseStock;
using ECommerceOrderManagement.API.Features.Inventory.ReserveStock;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Inventory.ChangeReorderLevel;
using ECommerceOrderManagement.Application.Features.Inventory.CreateInventoryItem;
using ECommerceOrderManagement.Application.Features.Inventory.DecreaseStock;
using ECommerceOrderManagement.Application.Features.Inventory.GetInventoryByProductId;
using ECommerceOrderManagement.Application.Features.Inventory.GetStockMovementHistory;
using ECommerceOrderManagement.Application.Features.Inventory.IncreaseStock;
using ECommerceOrderManagement.Application.Features.Inventory.ReserveStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Inventory;

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
        var command = new IncreaseStockCommand(productId, request.Quantity, request.Reason);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpPost("decrease")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DecreaseStock([FromRoute] Guid productId, [FromBody] DecreaseStockRequest request, CancellationToken cancellationToken)
    {
        var command = new DecreaseStockCommand(productId, request.Quantity, request.Reason);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpPut("reorder-level")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeReorderLevel([FromRoute] Guid productId, [FromBody] ChangeReorderLevelRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangeReorderLevelCommand(productId, request.ReorderLevel);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpGet("movements")]
    [ProducesResponseType<
    PagedResult<GetStockMovementHistoryItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStockMovementHistory([FromRoute] Guid productId, [FromQuery] GetStockMovementHistoryRequest request, CancellationToken cancellationToken)
    {
        var query = new GetStockMovementHistoryQuery(productId, request.Page, request.PageSize, request.Type, request.SortDirection);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpPost("reserve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ReserveStock([FromRoute] Guid productId, [FromBody] ReserveStockRequest request, CancellationToken cancellationToken)
    {
        var command = new ReserveStockCommand(productId, request.Quantity, request.Reason);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }
}