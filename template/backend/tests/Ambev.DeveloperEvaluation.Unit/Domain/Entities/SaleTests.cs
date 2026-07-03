using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity, covering totals, cancellation and validation.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that adding an item applies its discount and updates the sale total.
    /// </summary>
    [Fact(DisplayName = "Adding an item applies its discount and updates the sale total")]
    public void Given_Sale_When_ItemIsAdded_Then_TotalReflectsItemTotal()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale(itemCount: 0);
        var item = SaleTestData.GenerateItem(quantity: 10, unitPrice: 100m);

        // Act
        sale.AddItem(item);

        // Assert
        Assert.Equal(800m, item.Total);
        Assert.Equal(item.Total, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that the sale total ignores cancelled items when recalculated.
    /// </summary>
    [Fact(DisplayName = "The sale total ignores cancelled items")]
    public void Given_SaleWithItems_When_Recalculated_Then_CancelledItemsAreIgnored()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale(itemCount: 0);
        var first = SaleTestData.GenerateItem(quantity: 5, unitPrice: 100m);
        var second = SaleTestData.GenerateItem(quantity: 10, unitPrice: 100m);
        sale.AddItem(first);
        sale.AddItem(second);

        // Act
        first.Cancel();
        sale.RecalculateTotal();

        // Assert
        Assert.Equal(second.Total, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that cancelling the sale flags it as cancelled.
    /// </summary>
    [Fact(DisplayName = "Cancelling the sale flags it as cancelled")]
    public void Given_Sale_When_Cancelled_Then_IsCancelledIsTrue()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        sale.Cancel();

        // Assert
        Assert.True(sale.IsCancelled);
        Assert.NotNull(sale.UpdatedAt);
    }

    /// <summary>
    /// Tests that cancelling a single item removes its amount from the sale total.
    /// </summary>
    [Fact(DisplayName = "Cancelling an item removes its amount from the sale total")]
    public void Given_Sale_When_ItemIsCancelled_Then_TotalIsUpdated()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale(itemCount: 0);
        var kept = SaleTestData.GenerateItem(quantity: 4, unitPrice: 100m);
        var cancelled = SaleTestData.GenerateItem(quantity: 10, unitPrice: 100m);
        sale.AddItem(kept);
        sale.AddItem(cancelled);

        // Act
        sale.CancelItem(cancelled.Id);

        // Assert
        Assert.True(cancelled.IsCancelled);
        Assert.Equal(kept.Total, sale.TotalAmount);
    }

    /// <summary>
    /// Tests that cancelling an item that does not belong to the sale throws a domain exception.
    /// </summary>
    [Fact(DisplayName = "Cancelling an unknown item throws a domain exception")]
    public void Given_Sale_When_CancellingUnknownItem_Then_ThrowsDomainException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var act = () => sale.CancelItem(Guid.NewGuid());

        // Assert
        Assert.Throws<DomainException>(act);
    }

    /// <summary>
    /// Tests that a well-formed sale passes validation.
    /// </summary>
    [Fact(DisplayName = "A well-formed sale passes validation")]
    public void Given_ValidSale_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();

        // Act
        var result = sale.Validate();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    /// <summary>
    /// Tests that a sale with more than twenty identical items fails validation.
    /// </summary>
    [Fact(DisplayName = "A sale with more than twenty identical items fails validation")]
    public void Given_ItemAboveMaximumQuantity_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale(itemCount: 0);
        sale.AddItem(SaleTestData.GenerateItem(quantity: 21, unitPrice: 100m));

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
    }

    /// <summary>
    /// Tests that a sale without items fails validation.
    /// </summary>
    [Fact(DisplayName = "A sale without items fails validation")]
    public void Given_SaleWithoutItems_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale(itemCount: 0);

        // Act
        var result = sale.Validate();

        // Assert
        Assert.False(result.IsValid);
    }
}
