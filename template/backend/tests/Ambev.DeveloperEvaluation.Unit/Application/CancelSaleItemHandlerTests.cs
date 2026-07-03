using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleItemHandler"/> class.
/// </summary>
public class CancelSaleItemHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotifier _eventNotifier;
    private readonly CancelSaleItemHandler _handler;

    /// <summary>
    /// Initializes the test dependencies and the default mapper behavior.
    /// </summary>
    public CancelSaleItemHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventNotifier = Substitute.For<IEventNotifier>();
        _handler = new CancelSaleItemHandler(_saleRepository, _mapper, _eventNotifier);

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Sale>());
        _mapper.Map<SaleResult>(Arg.Any<Sale>()).Returns(new SaleResult());
    }

    /// <summary>
    /// Tests that cancelling an existing item flags it and publishes the ItemCancelled event.
    /// </summary>
    [Fact(DisplayName = "Given existing item When cancelling item Then cancels it and publishes the event")]
    public async Task Handle_ExistingItem_CancelsAndPublishesEvent()
    {
        // Given
        var sale = SaleTestData.GenerateValidSale(itemCount: 2);
        sale.Id = Guid.NewGuid();
        var itemId = sale.Items[0].Id;
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);

        // When
        await _handler.Handle(new CancelSaleItemCommand(sale.Id, itemId), CancellationToken.None);

        // Then
        sale.Items.First(item => item.Id == itemId).IsCancelled.Should().BeTrue();
        await _eventNotifier.Received(1).NotifyAsync(Arg.Any<SaleItemCancelledEvent>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that cancelling an item of a missing sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given missing sale When cancelling item Then throws KeyNotFoundException")]
    public async Task Handle_MissingSale_ThrowsKeyNotFoundException()
    {
        // Given
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(new CancelSaleItemCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    /// <summary>
    /// Tests that cancelling an item that does not belong to the sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given missing item When cancelling item Then throws KeyNotFoundException")]
    public async Task Handle_MissingItem_ThrowsKeyNotFoundException()
    {
        // Given
        var sale = SaleTestData.GenerateValidSale(itemCount: 1);
        sale.Id = Guid.NewGuid();
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var act = () => _handler.Handle(new CancelSaleItemCommand(sale.Id, Guid.NewGuid()), CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
