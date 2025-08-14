using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
/// <summary>
/// Handler for processing CreateSaleHandler requests
/// </summary>
public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{

    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="customerRepository">The customer repository</param>
/// <param name="branchRepository">The company repository</param>
    public CreateSaleHandler(ICustomerRepository customerRepository, IBranchRepository branchRepository)
    {
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetById(command.BranchId);
        if (branch is null)
            throw new DomainException($"Branch not found!");

        var customer = await _customerRepository.GetById(command.CustomerId);
        if (customer is null)
            throw new DomainException($"Customer not found!");

        var sale = Sale.Create(command.SaleNumber, customer.Id, customer.Name, branch.Id, branch.Name);
        var valid = sale.ValidateAsync();
        // TODO:
        // Implement repository to persist data
        return new CreateSaleResult()
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
        };

    }
}
