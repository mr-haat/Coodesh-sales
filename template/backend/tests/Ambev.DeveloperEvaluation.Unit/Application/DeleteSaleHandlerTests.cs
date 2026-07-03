using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly DeleteSaleHandler _handler;

    /// <summary>
    /// Initializes the test dependencies.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleHandler(_saleRepository);
    }

    /// <summary>
    /// Tests that deleting an existing sale returns a successful response.
    /// </summary>
    [Fact(DisplayName = "Given existing sale When deleting sale Then returns success")]
    public async Task Handle_ExistingSale_ReturnsSuccess()
    {
        // Given
        _saleRepository.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(true);

        // When
        var response = await _handler.Handle(new DeleteSaleCommand(Guid.NewGuid()), CancellationToken.None);

        // Then
        response.Success.Should().BeTrue();
    }

    /// <summary>
    /// Tests that deleting a missing sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given missing sale When deleting sale Then throws KeyNotFoundException")]
    public async Task Handle_MissingSale_ThrowsKeyNotFoundException()
    {
        // Given
        _saleRepository.DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(false);

        // When
        var act = () => _handler.Handle(new DeleteSaleCommand(Guid.NewGuid()), CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
