using Ambev.DeveloperEvaluation.Application.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSaleItems;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new Sale
    /// </summary>
    /// <param name="request">The sale creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        await new CreateSaleRequestValidator()
            .ValidateAsync(request, cancellationToken)
            .ThrowIfInvalid();

        var command = _mapper.Map<CreateSaleCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(response)
        });
    }

    /// <summary>
    /// Adds a new Sale Item to the Sale
    /// </summary>
    /// <param name="saleId">The sale identifier to create an item at</param>
    /// <param name="request">The sale item creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale item identifier</returns>
    [HttpPost("{saleId:guid}/Items")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddSaleItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSaleItem(Guid saleId, [FromBody] AddSaleItemRequest request, CancellationToken cancellationToken)
    {
        await new AddSaleItemRequestValidator()
            .ValidateAsync(request, cancellationToken)
            .ThrowIfInvalid();

        if (saleId == Guid.Empty)
            throw new ValidationException($"{nameof(saleId)} must be present!");

        var command = _mapper.Map<AddSaleItemCommand>(request);
        command.SaleId = saleId;
        var response = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<AddSaleItemResponse>
        {
            Success = true,
            Message = "Sale Item created successfully",
            Data = _mapper.Map<AddSaleItemResponse>(response)
        });
    }

    /// <summary>
    /// Cancel a Sale Item on the Sale
    /// </summary>
    /// <param name="saleId">The sale identifier to remove and item from</param>
    /// <param name="productId">The product id from the sale item to be canceled</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success if item is deleted</returns>
    [HttpDelete("{saleId:guid}/Item/{productId:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddSaleItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelSaleItem(Guid saleId, Guid productId, CancellationToken cancellationToken)
    {
        var request = new CancelSaleItemRequest
        {
            SaleId = saleId,
            ProductId = productId
        };
        await new CancelSaleItemRequestValidator()
            .ValidateAsync(request, cancellationToken)
            .ThrowIfInvalid();

        var command = _mapper.Map<CancelSaleItemCommand>(request);

        await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Product cancelled from sale successfully!"
        });
    }

    /// <summary>
    /// Gets a list of all the sales without its item contents. To get the items, query for them on the ListSaleItems
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListSaleResponseEntry>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListSales([PaginationFromQuery] PaginateRequest pagination, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<ListSaleCommand>(pagination);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponseWithData<ListSaleResponse>
        {
            Success = true,
            Data = _mapper.Map<ListSaleResponse>(result),
        });
    }

    /// <summary>
    /// Gets a list of all the sales items from a Sale.
    /// </summary>
    /// <param name="saleId">The sale identifier</param>
    /// <param name="pagination"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{saleId:guid}/Items")]
    [ProducesResponseType(typeof(ApiResponseWithData<ListSaleResponseEntry>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListSales(
        Guid saleId,
        [PaginationFromQuery] PaginateRequest pagination, CancellationToken cancellationToken)
    {
        var request = new ListSaleItemsRequest
        {
            SaleId = saleId
        };

        await new ListSaleItemsRequestValidator()
            .ValidateAsync(request, cancellationToken)
            .ThrowIfInvalid();

        var command = _mapper.Map<ListSaleItemsCommand>(pagination);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponseWithData<ListSaleItemsResponse>
        {
            Success = true,
            Data = _mapper.Map<ListSaleItemsResponse>(result),
        });
    }
}