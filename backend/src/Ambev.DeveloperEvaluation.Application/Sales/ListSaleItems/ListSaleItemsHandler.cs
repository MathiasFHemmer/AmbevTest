using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSaleItems;

/// <summary>
/// Handler for processing ListSaleItemCommand requests
/// </summary>
public sealed class ListSaleItemsHandler : IRequestHandler<ListSaleItemsCommand, ListSaleItemsResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public ListSaleItemsHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<ListSaleItemsResult> Handle(ListSaleItemsCommand command, CancellationToken cancellationToken)
    {
        var items = await _saleRepository.ListSaleItemsBySaleId(command.SaleId, command.Pagination, cancellationToken);
        return _mapper.Map<ListSaleItemsResult>(items);
    }
}
