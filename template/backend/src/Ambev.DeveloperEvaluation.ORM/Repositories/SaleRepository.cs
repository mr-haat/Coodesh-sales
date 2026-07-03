using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Retrieves a page of sales matching the given filter
    /// </summary>
    /// <param name="filter">The pagination, ordering and filtering options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The matching sales and the total number of sales for the filter</returns>
    public async Task<(IReadOnlyList<Sale> Sales, int TotalCount)> ListAsync(SaleListFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.Include(s => s.Items).AsQueryable();

        if (filter.CustomerId.HasValue)
            query = query.Where(s => s.CustomerId == filter.CustomerId.Value);

        if (filter.BranchId.HasValue)
            query = query.Where(s => s.BranchId == filter.BranchId.Value);

        if (filter.IsCancelled.HasValue)
            query = query.Where(s => s.IsCancelled == filter.IsCancelled.Value);

        if (filter.MinSaleDate.HasValue)
            query = query.Where(s => s.SaleDate >= filter.MinSaleDate.Value);

        if (filter.MaxSaleDate.HasValue)
            query = query.Where(s => s.SaleDate <= filter.MaxSaleDate.Value);

        if (filter.MinTotalAmount.HasValue)
            query = query.Where(s => s.TotalAmount >= filter.MinTotalAmount.Value);

        if (filter.MaxTotalAmount.HasValue)
            query = query.Where(s => s.TotalAmount <= filter.MaxTotalAmount.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var sales = await ApplyOrdering(query, filter.Order)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (sales, totalCount);
    }

    /// <summary>
    /// Updates an existing sale in the database
    /// </summary>
    /// <param name="sale">The sale to update (already tracked by the context)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>
    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    private static readonly Dictionary<string, Expression<Func<Sale, object>>> SortSelectors =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["saleNumber"] = s => s.SaleNumber,
            ["saleDate"] = s => s.SaleDate,
            ["totalAmount"] = s => s.TotalAmount,
            ["customerName"] = s => s.CustomerName,
            ["branchName"] = s => s.BranchName
        };

    private static IQueryable<Sale> ApplyOrdering(IQueryable<Sale> query, string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query.OrderByDescending(s => s.SaleDate);

        IOrderedQueryable<Sale>? ordered = null;
        foreach (var clause in order.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var tokens = clause.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!SortSelectors.TryGetValue(tokens[0], out var selector))
                continue;

            var descending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            if (ordered is null)
                ordered = descending ? query.OrderByDescending(selector) : query.OrderBy(selector);
            else
                ordered = descending ? ordered.ThenByDescending(selector) : ordered.ThenBy(selector);
        }

        return ordered ?? query.OrderByDescending(s => s.SaleDate);
    }

    /// <summary>
    /// Deletes a sale from the database
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
