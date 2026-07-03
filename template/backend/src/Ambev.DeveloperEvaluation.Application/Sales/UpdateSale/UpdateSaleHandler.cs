using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests.
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, SaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotifier _eventNotifier;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="eventNotifier">The event notifier</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventNotifier eventNotifier)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventNotifier = eventNotifier;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request.
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<SaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        var duplicate = await _saleRepository.GetBySaleNumberAsync(command.SaleNumber, cancellationToken);
        if (duplicate != null && duplicate.Id != sale.Id)
            throw new InvalidOperationException($"A sale with number {command.SaleNumber} already exists");

        sale.SaleNumber = command.SaleNumber;
        sale.SaleDate = command.SaleDate;
        sale.CustomerId = command.CustomerId;
        sale.CustomerName = command.CustomerName;
        sale.BranchId = command.BranchId;
        sale.BranchName = command.BranchName;

        sale.Items.Clear();
        foreach (var item in command.Items)
            sale.AddItem(_mapper.Map<SaleItem>(item));

        sale.UpdatedAt = DateTime.UtcNow;

        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        await _eventNotifier.NotifyAsync(new SaleModifiedEvent(updatedSale), cancellationToken);

        return _mapper.Map<SaleResult>(updatedSale);
    }
}
