using Ambev.DeveloperEvaluation.Application.Sales.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Represents a page of sales returned by the application layer.
/// </summary>
public class ListSalesResult
{
    /// <summary>
    /// The sales in the current page.
    /// </summary>
    public IReadOnlyList<SaleResult> Sales { get; set; } = new List<SaleResult>();

    /// <summary>
    /// The current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The page size used to build the page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The total number of sales matching the filter.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// The total number of pages available for the filter.
    /// </summary>
    public int TotalPages { get; set; }
}
