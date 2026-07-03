using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotifier _eventNotifier;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes the test dependencies and the default mapper behavior.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventNotifier = Substitute.For<IEventNotifier>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _eventNotifier);

        _mapper.Map<SaleItem>(Arg.Any<UpdateSaleItemCommand>())
            .Returns(call =>
            {
                var item = call.Arg<UpdateSaleItemCommand>();
                return new SaleItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
            });

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Sale>());

        _mapper.Map<SaleResult>(Arg.Any<Sale>()).Returns(new SaleResult());
    }

    private static UpdateSaleCommand ValidCommand(Guid id) => new()
    {
        Id = id,
        SaleNumber = "SALE-0001",
        SaleDate = DateTime.UtcNow,
        CustomerId = Guid.NewGuid(),
        CustomerName = "Customer",
        BranchId = Guid.NewGuid(),
        BranchName = "Branch",
        Items = new List<UpdateSaleItemCommand>
        {
            new() { ProductId = Guid.NewGuid(), ProductName = "Product", Quantity = 5, UnitPrice = 100m }
        }
    };

    /// <summary>
    /// Tests that updating a missing sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given missing sale When updating sale Then throws KeyNotFoundException")]
    public async Task Handle_MissingSale_ThrowsKeyNotFoundException()
    {
        // Given
        var command = ValidCommand(Guid.NewGuid());
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    /// <summary>
    /// Tests that a valid update publishes the SaleModified event.
    /// </summary>
    [Fact(DisplayName = "Given valid update When updating sale Then publishes SaleModified event")]
    public async Task Handle_ValidRequest_PublishesEvent()
    {
        // Given
        var id = Guid.NewGuid();
        var command = ValidCommand(id);
        _saleRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(new Sale { Id = id });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        await _eventNotifier.Received(1).NotifyAsync(Arg.Any<SaleModifiedEvent>(), Arg.Any<CancellationToken>());
    }
}
