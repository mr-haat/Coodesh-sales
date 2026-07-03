namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Describes the pagination, ordering and filtering options used when listing sales.
/// </summary>
public class SaleListFilter
{
    /// <summary>
    /// The page number to retrieve, starting at 1.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of sales to retrieve per page.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// The ordering expression, for example "saleDate desc, saleNumber asc".
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Filters the sales by customer.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Filters the sales by branch.
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Filters the sales by their cancelled state.
    /// </summary>
    public bool? IsCancelled { get; set; }

    /// <summary>
    /// Filters the sales made on or after this date.
    /// </summary>
    public DateTime? MinSaleDate { get; set; }

    /// <summary>
    /// Filters the sales made on or before this date.
    /// </summary>
    public DateTime? MaxSaleDate { get; set; }

    /// <summary>
    /// Filters the sales with a total amount greater than or equal to this value.
    /// </summary>
    public decimal? MinTotalAmount { get; set; }

    /// <summary>
    /// Filters the sales with a total amount less than or equal to this value.
    /// </summary>
    public decimal? MaxTotalAmount { get; set; }
}
