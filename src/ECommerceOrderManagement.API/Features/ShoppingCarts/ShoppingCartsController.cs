using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.ShoppingCarts.AddShoppingCartItem;
using ECommerceOrderManagement.API.Features.ShoppingCarts.UpdateShoppingCartItem;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Features.ShoppingCarts;
using ECommerceOrderManagement.Application.Features.ShoppingCarts.AddShoppingCartItem;
using ECommerceOrderManagement.Application.Features.ShoppingCarts.DeleteShoppingCartItem;
using ECommerceOrderManagement.Application.Features.ShoppingCarts.GetShoppingCart;
using ECommerceOrderManagement.Application.Features.ShoppingCarts.UpdateShoppingCartItem;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.ShoppingCarts;

[Authorize(Roles = ApplicationRoles.Customer)]
[Route("api/cart")]
public sealed class ShoppingCartsController(ISender sender) : BaseApiController
{

    [HttpPost("items")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddItem([FromBody] AddShoppingCartItemRequest request, CancellationToken cancellationToken)
    {
        var command = new AddShoppingCartItemCommand(request.ProductId, request.Quantity);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpGet]
    [ProducesResponseType<ShoppingCartReadModel>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetShoppingCart(CancellationToken cancellationToken)
    {
        var query = new GetShoppingCartQuery();

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result);
    }



    [HttpPut("items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid productId, [FromBody] UpdateShoppingCartItemRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateShoppingCartItemCommand(productId, request.Quantity);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }



    [HttpDelete("items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid productId, CancellationToken cancellationToken)
    {
        var command = new DeleteShoppingCartItemCommand(productId);

        var result = await sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }

}