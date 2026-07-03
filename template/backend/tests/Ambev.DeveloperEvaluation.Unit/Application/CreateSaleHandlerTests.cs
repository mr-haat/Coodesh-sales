using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventNotifier _eventNotifier;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes the test dependencies and the default mapper behavior.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventNotifier = Substitute.For<IEventNotifier>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _eventNotifier);

        _mapper.Map<SaleItem>(Arg.Any<CreateSaleItemCommand>())
            .Returns(call =>
            {
                var item = call.Arg<CreateSaleItemCommand>();
                return new SaleItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
            });

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(call => call.Arg<Sale>());

        _mapper.Map<SaleResult>(Arg.Any<Sale>()).Returns(new SaleResult());
    }

    /// <summary>
    /// Tests that a valid command persists the sale and returns a result.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then persists and returns result")]
    public async Task Handle_ValidRequest_ReturnsResult()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a valid command publishes the SaleCreated event.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then publishes SaleCreated event")]
    public async Task Handle_ValidRequest_PublishesEvent()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _eventNotifier.Received(1).NotifyAsync(Arg.Any<SaleCreatedEvent>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the discount rules are applied to the total of the persisted sale.
    /// </summary>
    [Fact(DisplayName = "Given items When creating sale Then applies discount to the persisted total")]
    public async Task Handle_ValidRequest_AppliesDiscountToTotal()
    {
        // Given
        var command = new CreateSaleCommand
        {
            SaleNumber = "SALE-0001",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Customer",
            BranchId = Guid.NewGuid(),
            BranchName = "Branch",
            Items =
            [
                new() { ProductId = Guid.NewGuid(), ProductName = "Product", Quantity = 10, UnitPrice = 100m }
            ]
        };

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(sale => sale.TotalAmount == 800m),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that creating a sale with an existing number throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given existing sale number When creating sale Then throws InvalidOperationException")]
    public async Task Handle_DuplicateSaleNumber_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
            .Returns(new Sale());

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    /// <summary>
    /// Tests that an invalid command throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }
}
