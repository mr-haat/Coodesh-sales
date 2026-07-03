using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for processing ListSalesCommand requests.
/// </summary>
public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly PaginationSettings _pagination;

    /// <summary>
    /// Initializes a new instance of ListSalesHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="paginationOptions">The pagination settings</param>
    public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper, IOptions<PaginationSettings> paginationOptions)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _pagination = paginationOptions.Value;
    }

    /// <summary>
    /// Handles the ListSalesCommand request.
    /// </summary>
    /// <param name="command">The ListSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The requested page of sales</returns>
    public async Task<ListSalesResult> Handle(ListSalesCommand command, CancellationToken cancellationToken)
    {
        var page = command.Page < 1 ? 1 : command.Page;
        var pageSize = command.PageSize <= 0
            ? _pagination.DefaultPageSize
            : Math.Min(command.PageSize, _pagination.MaxPageSize);

        var filter = new SaleListFilter
        {
            Page = page,
            PageSize = pageSize,
            Order = command.Order,
            CustomerId = command.CustomerId,
            BranchId = command.BranchId,
            IsCancelled = command.IsCancelled,
            MinSaleDate = command.MinSaleDate,
            MaxSaleDate = command.MaxSaleDate,
            MinTotalAmount = command.MinTotalAmount,
            MaxTotalAmount = command.MaxTotalAmount
        };

        var (sales, totalCount) = await _saleRepository.ListAsync(filter, cancellationToken);

        return new ListSalesResult
        {
            Sales = _mapper.Map<List<SaleResult>>(sales),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}
