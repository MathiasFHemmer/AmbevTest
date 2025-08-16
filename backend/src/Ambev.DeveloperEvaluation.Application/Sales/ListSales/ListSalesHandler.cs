using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;
/// <summary>
/// Handler for processing ListSalesHandler requests
/// </summary>
public sealed class ListSalesHandler : IRequestHandler<ListSaleCommand, ListSaleResult>
{

    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;


    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// /// <param name="mapper">The mapper</param>
    public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the ListSaleCommand request
    /// </summary>
    /// <param name="command">The ListSaleCommand command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A ListSaleResult that contains paginated information about the query</returns>
    public async Task<ListSaleResult> Handle(ListSaleCommand command, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.ListSales(command.Pagination, cancellationToken);
        return _mapper.Map<ListSaleResult>(sales);
    }
}
