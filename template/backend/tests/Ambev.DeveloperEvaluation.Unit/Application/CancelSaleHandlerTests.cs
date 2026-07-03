using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CancelSaleHandler"/> class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotifier _eventNotifier;
    private readonly CancelSaleHandler _handler;

    /// <summary>
    /// Initializes the test dependencies and the default mapper behavior.
    /// </summary>
    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventNotifier = Substitute.For<IEventNotifier>();
        _handler = new CancelSaleHandler(_saleRepository, _mapper, _eventNotifier);

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Sale>());
        _mapper.Map<SaleResult>(Arg.Any<Sale>()).Returns(new SaleResult());
    }

    /// <summary>
    /// Tests that cancelling an existing sale flags it and publishes the SaleCancelled event.
    /// </summary>
    [Fact(DisplayName = "Given existing sale When cancelling sale Then cancels it and publishes the event")]
    public async Task Handle_ExistingSale_CancelsAndPublishesEvent()
    {
        // Given
        var sale = new Sale { Id = Guid.NewGuid() };
        _saleRepository.GetByIdAsync(sale.Id, Arg.Any<CancellationToken>()).Returns(sale);

        // When
        await _handler.Handle(new CancelSaleCommand(sale.Id), CancellationToken.None);

        // Then
        sale.IsCancelled.Should().BeTrue();
        await _eventNotifier.Received(1).NotifyAsync(Arg.Any<SaleCancelledEvent>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that cancelling a missing sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given missing sale When cancelling sale Then throws KeyNotFoundException")]
    public async Task Handle_MissingSale_ThrowsKeyNotFoundException()
    {
        // Given
        _saleRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(new CancelSaleCommand(Guid.NewGuid()), CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
