using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Handler for processing CancelSaleItemCommand requests.
/// </summary>
public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotifier _eventNotifier;

    /// <summary>
    /// Initializes a new instance of CancelSaleItemHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="eventNotifier">The event notifier</param>
    public CancelSaleItemHandler(ISaleRepository saleRepository, IMapper mapper, IEventNotifier eventNotifier)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventNotifier = eventNotifier;
    }

    /// <summary>
    /// Handles the CancelSaleItemCommand request.
    /// </summary>
    /// <param name="request">The CancelSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<SaleResult> Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {request.SaleId} not found");

        if (sale.Items.All(item => item.Id != request.ItemId))
            throw new KeyNotFoundException($"Item with ID {request.ItemId} was not found in sale {request.SaleId}");

        sale.CancelItem(request.ItemId);

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        await _eventNotifier.NotifyAsync(new SaleItemCancelledEvent(updatedSale, request.ItemId), cancellationToken);

        return _mapper.Map<SaleResult>(updatedSale);
    }
}
