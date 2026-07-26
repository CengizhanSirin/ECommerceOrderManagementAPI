using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Catalog.Products.CreateProduct;
using ECommerceOrderManagement.Application.Features.Catalog.Products.CreateProduct;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrderManagement.API.Features.Catalog.Products;

[Route("api/products")]
public sealed class ProductsController : BaseApiController
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType<CreateProductResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.Name, request.Slug, request.Sku, request.PriceAmount, request.Currency, request.CategoryId, request.BrandId, request.Description, request.MainImageUrl);

        var result = await _sender.Send(command, cancellationToken);

        return HandleCreatedResult(result,
            productId => new CreateProductResponse
            {
                Id = productId
            },
            response => $"/api/products/{response.Id}");
    }

    [HttpGet("{productId:guid}")]
    [ProducesResponseType<GetProductByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(Guid productId, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(productId);

        var result = await _sender.Send(query, cancellationToken);

        return HandleResult(result);
    }
}