namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

/// <summary>
/// API response model representing a sale.
/// </summary>
public class SaleResponse
{
    /// <summary>
    /// The unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The number that identifies the sale.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The identifier of the customer the sale belongs to.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The denormalized name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the branch where the sale was made.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// The denormalized name of the branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// The total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Indicates whether the sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// The items that compose the sale.
    /// </summary>
    public List<SaleItemResponse> Items { get; set; } = new();
}

/// <summary>
/// API response model representing a single item of a sale.
/// </summary>
public class SaleItemResponse
{
    /// <summary>
    /// The unique identifier of the item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The identifier of the product being sold.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The denormalized name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product being sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The discount amount applied to the item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// The total amount of the item after the discount.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Indicates whether the item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}
