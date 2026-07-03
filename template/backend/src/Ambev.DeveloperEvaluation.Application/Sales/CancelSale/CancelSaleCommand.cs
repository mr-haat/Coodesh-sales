using Ambev.DeveloperEvaluation.Application.Sales.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Command for cancelling a sale.
/// </summary>
public record CancelSaleCommand : IRequest<SaleResult>
{
    /// <summary>
    /// The unique identifier of the sale to cancel.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Initializes a new instance of CancelSaleCommand.
    /// </summary>
    /// <param name="id">The identifier of the sale to cancel.</param>
    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }
}
