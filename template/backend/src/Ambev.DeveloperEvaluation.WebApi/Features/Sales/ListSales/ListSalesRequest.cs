using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Request model for listing sales, bound from the query string following the
/// pagination, ordering and filtering conventions of the API.
/// </summary>
public class ListSalesRequest
{
    /// <summary>
    /// Gets or sets the page number to retrieve.
    /// </summary>
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the requested page size.
    /// </summary>
    [FromQuery(Name = "_size")]
    public int Size { get; set; }

    /// <summary>
    /// Gets or sets the ordering expression, for example "saleDate desc, saleNumber asc".
    /// </summary>
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }

    /// <summary>
    /// Gets or sets the customer filter.
    /// </summary>
    [FromQuery(Name = "customerId")]
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch filter.
    /// </summary>
    [FromQuery(Name = "branchId")]
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the cancelled state filter.
    /// </summary>
    [FromQuery(Name = "isCancelled")]
    public bool? IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the minimum sale date filter.
    /// </summary>
    [FromQuery(Name = "_minSaleDate")]
    public DateTime? MinSaleDate { get; set; }

    /// <summary>
    /// Gets or sets the maximum sale date filter.
    /// </summary>
    [FromQuery(Name = "_maxSaleDate")]
    public DateTime? MaxSaleDate { get; set; }

    /// <summary>
    /// Gets or sets the minimum total amount filter.
    /// </summary>
    [FromQuery(Name = "_minTotalAmount")]
    public decimal? MinTotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the maximum total amount filter.
    /// </summary>
    [FromQuery(Name = "_maxTotalAmount")]
    public decimal? MaxTotalAmount { get; set; }
}
