using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a single product line within a sale.
/// The product is referenced through the External Identities pattern, storing the
/// product identifier together with a denormalized description.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the sale this item belongs to.
    /// </summary>
    public Guid SaleId { get; set; }

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

    /// <summary>
    /// Gets or sets the discount amount applied to the item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount of the item after the discount.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Applies the quantity-based discount rules and recalculates the item total.
    /// </summary>
    /// <remarks>
    /// The applicable discount rate is defined by <see cref="SaleDiscountPolicy"/>.
    /// </remarks>
    public void ApplyDiscountRules()
    {
        var grossAmount = Quantity * UnitPrice;
        Discount = Math.Round(grossAmount * SaleDiscountPolicy.GetRateFor(Quantity), 2);
        Total = grossAmount - Discount;
    }

    /// <summary>
    /// Marks the item as cancelled.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
    }
}
