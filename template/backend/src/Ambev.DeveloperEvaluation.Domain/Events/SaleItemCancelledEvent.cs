using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Event raised when a single item of a sale is cancelled.
/// </summary>
public class SaleItemCancelledEvent
{
    /// <summary>
    /// The sale that owns the cancelled item.
    /// </summary>
    public Sale Sale { get; }

    /// <summary>
    /// The identifier of the item that was cancelled.
    /// </summary>
    public Guid ItemId { get; }

    /// <summary>
    /// Initializes a new instance of SaleItemCancelledEvent.
    /// </summary>
    /// <param name="sale">The sale that owns the cancelled item.</param>
    /// <param name="itemId">The identifier of the item that was cancelled.</param>
    public SaleItemCancelledEvent(Sale sale, Guid itemId)
    {
        Sale = sale;
        ItemId = itemId;
    }
}
