using Ambev.DeveloperEvaluation.Application.Sales.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Command for listing sales with pagination, ordering and filtering.
/// </summary>
public class ListSalesCommand : IRequest<ListSalesResult>
{
    /// <summary>
    /// Gets or sets the page number to retrieve.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the requested page size. When zero or negative the configured default is used.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the ordering expression, for example "saleDate desc, saleNumber asc".
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Gets or sets the customer filter.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch filter.
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the cancelled state filter.
    /// </summary>
    public bool? IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the minimum sale date filter.
    /// </summary>
    public DateTime? MinSaleDate { get; set; }

    /// <summary>
    /// Gets or sets the maximum sale date filter.
    /// </summary>
    public DateTime? MaxSaleDate { get; set; }

    /// <summary>
    /// Gets or sets the minimum total amount filter.
    /// </summary>
    public decimal? MinTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the maximum total amount filter.
    /// </summary>
    public decimal? MaxTotalAmount { get; set; }
}
