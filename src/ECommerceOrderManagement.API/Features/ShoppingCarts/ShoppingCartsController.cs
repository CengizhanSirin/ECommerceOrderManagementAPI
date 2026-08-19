using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.ShoppingCarts.AddShoppingCartItem;
using ECommerceOrderManagement.Application.Common.Authorization;
using ECommerceOrderManagement.Application.Features.ShoppingCarts.AddShoppingCartItem;
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

}