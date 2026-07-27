using ECommerceOrderManagement.API.Common.Controllers;
using ECommerceOrderManagement.API.Features.Catalog.Products.CreateProduct;
using ECommerceOrderManagement.API.Features.Catalog.Products.GetProducts;
using ECommerceOrderManagement.API.Features.Catalog.Products.UpdateProduct;
using ECommerceOrderManagement.Application.Common.Pagination;
using ECommerceOrderManagement.Application.Features.Catalog.Products.CreateProduct;
using ECommerceOrderManagement.Application.Features.Catalog.Products.DeleteProduct;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProductById;
using ECommerceOrderManagement.Application.Features.Catalog.Products.GetProducts;
using ECommerceOrderManagement.Application.Features.Catalog.Products.UpdateProduct;
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

    [HttpPut("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(productId,
            request.Name,
            request.Slug,
            request.Sku,
            request.PriceAmount,
            request.Currency,
            request.CategoryId,
            request.BrandId,
            request.Description,
            request.MainImageUrl);

        var result = await _sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }

    [HttpDelete("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid productId, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(productId);

        var result = await _sender.Send(command, cancellationToken);

        return HandleNoContent(result);
    }

    [HttpGet]
    [ProducesResponseType<PagedResult<GetProductsItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsRequest request, CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.CategoryId,
            request.BrandId,
            request.IsActive,
            request.SortBy,
            request.SortDirection);

        var result = await _sender.Send(query, cancellationToken);

        return HandleResult(result);
    }
}