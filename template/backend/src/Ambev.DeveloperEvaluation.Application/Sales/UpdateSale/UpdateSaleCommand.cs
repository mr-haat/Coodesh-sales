using Ambev.DeveloperEvaluation.Application.Sales.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command for updating an existing sale and its items.
/// </summary>
public class UpdateSaleCommand : IRequest<SaleResult>
{
    /// <summary>
    /// Gets or sets the identifier of the sale to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the number that identifies the sale.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the customer the sale belongs to.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the denormalized name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the branch where the sale was made.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the denormalized name of the branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the items that compose the sale.
    /// </summary>
    public List<UpdateSaleItemCommand> Items { get; set; } = new();
}

/// <summary>
/// Represents a single item of an update sale command.
/// </summary>
public class UpdateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the identifier of the product being sold.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the denormalized name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product being sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
}
